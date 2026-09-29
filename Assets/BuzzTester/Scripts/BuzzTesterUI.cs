using Buzz.Input;
using UnityEngine;

namespace Buzz.Diagnostics
{
    public sealed class BuzzTesterUI : MonoBehaviour
    {
        private enum Language { Portuguese, English }

        private static readonly string[] PortugueseButtonNames = { "BUZZ", "AZUL", "LARANJA", "VERDE", "AMARELO" };
        private static readonly string[] EnglishButtonNames = { "BUZZ", "BLUE", "ORANGE", "GREEN", "YELLOW" };
        private static readonly Color[] ButtonColors =
        {
            new Color(0.90f, 0.12f, 0.16f), new Color(0.12f, 0.48f, 0.94f),
            new Color(1.00f, 0.48f, 0.08f), new Color(0.16f, 0.75f, 0.34f),
            new Color(0.98f, 0.82f, 0.10f)
        };

        private GUIStyle titleStyle, subtitleStyle, textStyle, smallStyle, panelStyle, buttonStyle;
        private Texture2D portugalFlag, unitedKingdomFlag, selectedFrame, normalFrame;
        private Language language = Language.Portuguese;
        private bool hasLastAction;
        private int lastPlayer;
        private BuzzButton lastButton;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (FindFirstObjectByType<BuzzTesterUI>() == null)
                new GameObject("Buzz Tester UI").AddComponent<BuzzTesterUI>();
        }

        private void Start() => Subscribe();

        private void Subscribe()
        {
            var input = BuzzInputManager.Instance;
            if (input == null)
            {
                Invoke(nameof(Subscribe), 0.1f);
                return;
            }
            input.ButtonPressed -= OnButtonPressed;
            input.ButtonPressed += OnButtonPressed;
        }

        private void OnButtonPressed(int player, BuzzButton button)
        {
            hasLastAction = true;
            lastPlayer = player;
            lastButton = button;
        }

        private void OnGUI()
        {
            EnsureStyles();
            var input = BuzzInputManager.Instance;
            if (input == null) return;

            const float designWidth = 1280f, designHeight = 720f;
            var scale = Mathf.Min(Screen.width / designWidth, Screen.height / designHeight);
            var oldMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(
                new Vector3((Screen.width - designWidth * scale) * 0.5f, (Screen.height - designHeight * scale) * 0.5f, 0),
                Quaternion.identity, new Vector3(scale, scale, 1));

            GUI.backgroundColor = new Color(0.045f, 0.06f, 0.10f);
            GUI.Box(new Rect(0, 0, designWidth, designHeight), GUIContent.none);
            GUI.backgroundColor = Color.white;

            GUI.Label(new Rect(48, 24, 650, 44), "BUZZ TESTER", titleStyle);
            GUI.Label(new Rect(50, 67, 760, 30), T("Teste independente da ligação dos comandos sem jogos", "Independent controller connection test without games"), subtitleStyle);
            DrawLanguageButton(new Rect(872, 38, 58, 40), portugalFlag, Language.Portuguese, "Português");
            DrawLanguageButton(new Rect(940, 38, 58, 40), unitedKingdomFlag, Language.English, "English");

            GUI.color = input.IsConnected ? new Color(0.45f, 1f, 0.62f) : new Color(1f, 0.55f, 0.45f);
            GUI.Label(new Rect(50, 105, 930, 28), LocaliseStatus(input.Status), textStyle);
            GUI.color = Color.white;
            if (GUI.Button(new Rect(1020, 38, 210, 48), T("VOLTAR A LIGAR", "RECONNECT"))) input.Connect();

            for (var player = 1; player <= 4; player++)
                DrawPlayer(input, player, 48 + (player - 1) * 300, 150);

            var lastAction = hasLastAction
                ? T("Jogador ", "Player ") + lastPlayer + ": " + ButtonNames[(int)lastButton]
                : T("Ainda não foi premido nenhum botão.", "No button has been pressed yet.");
            GUI.Label(new Rect(50, 604, 690, 28), T("Último botão: ", "Last button: ") + lastAction, textStyle);
            GUI.Label(new Rect(790, 604, 440, 28), T("Relatórios HID recebidos: ", "HID reports received: ") + input.ReportCount, textStyle);
            GUI.Label(new Rect(50, 638, 1180, 24), T("Último relatório HID: ", "Last HID report: ") + input.LastReportHex, smallStyle);
            GUI.Label(new Rect(50, 674, 1180, 24), T(
                "Podes testar apenas os comandos que têm pilhas. Não é necessário ligar os quatro.",
                "You can test only the controllers that have batteries. All four are not required."), smallStyle);
            GUI.matrix = oldMatrix;
        }

        private string[] ButtonNames => language == Language.Portuguese ? PortugueseButtonNames : EnglishButtonNames;
        private string T(string portuguese, string english) => language == Language.Portuguese ? portuguese : english;

        private string LocaliseStatus(string status)
        {
            if (language == Language.Portuguese) return status;
            return status.Replace("A iniciar…", "Starting…")
                .Replace("A abrir o recetor Wbuzz…", "Opening the Wbuzz receiver…")
                .Replace(" inicializado. Carrega num botão.", " initialised. Press a button.")
                .Replace("Recetor Wbuzz (VID 054C / PID 1000) não encontrado.", "Wbuzz receiver (VID 054C / PID 1000) not found.")
                .Replace(" Nova tentativa automática…", " Retrying automatically…")
                .Replace("Ligação perdida: ", "Connection lost: ")
                .Replace("O teste Wbuzz está atualmente preparado para Windows.", "The Wbuzz test is currently available for Windows.")
                .Replace("O Wbuzz foi encontrado", "Wbuzz was found")
                .Replace("interface(s)), mas não foi possível abrir a ligação HID.", "interface(s)), but the HID connection could not be opened.");
        }

        private void DrawLanguageButton(Rect rect, Texture2D flag, Language target, string tooltip)
        {
            GUI.DrawTexture(rect, language == target ? selectedFrame : normalFrame);
            GUI.DrawTexture(new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6), flag, ScaleMode.StretchToFill);
            if (GUI.Button(rect, new GUIContent(string.Empty, tooltip), GUIStyle.none)) language = target;
        }

        private void DrawPlayer(BuzzInputManager input, int player, float x, float y)
        {
            // GUI.backgroundColor tints style textures; reset it before every panel.
            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            GUI.Box(new Rect(x, y, 270, 425), GUIContent.none, panelStyle);
            GUI.Label(new Rect(x + 22, y + 16, 220, 36), T("JOGADOR ", "PLAYER ") + player, titleStyle);
            for (var button = 0; button < 5; button++)
            {
                var pressed = input.GetButton(player, (BuzzButton)button);
                var rect = new Rect(x + 25, y + 68 + button * 61, 220, 45);
                GUI.backgroundColor = pressed ? ButtonColors[button] : new Color(0.16f, 0.19f, 0.27f);
                GUI.color = pressed || button == 0 ? Color.white : new Color(0.78f, 0.82f, 0.9f);
                GUI.Box(rect, pressed ? ButtonNames[button] + "  ✓" : ButtonNames[button], buttonStyle);
            }
            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            if (GUI.Button(new Rect(x + 25, y + 382, 220, 30), T("TESTAR LUZ", "TEST LIGHT")))
            {
                input.SetPlayerLeds(player == 1, player == 2, player == 3, player == 4);
                CancelInvoke(nameof(ClearLeds));
                Invoke(nameof(ClearLeds), 1.5f);
            }
        }

        private void ClearLeds() => BuzzInputManager.Instance?.SetPlayerLeds(false, false, false, false);

        private void OnDestroy()
        {
            CancelInvoke();
            if (BuzzInputManager.Instance != null) BuzzInputManager.Instance.ButtonPressed -= OnButtonPressed;
        }

        private void EnsureStyles()
        {
            if (titleStyle != null) return;
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            subtitleStyle = new GUIStyle(GUI.skin.label) { fontSize = 18, normal = { textColor = new Color(0.70f, 0.76f, 0.88f) } };
            textStyle = new GUIStyle(GUI.skin.label) { fontSize = 17, normal = { textColor = new Color(0.80f, 0.84f, 0.92f) } };
            smallStyle = new GUIStyle(textStyle) { fontSize = 15 };
            panelStyle = new GUIStyle(GUI.skin.box) { normal = { background = MakeTexture(new Color(0.09f, 0.115f, 0.18f)) } };
            buttonStyle = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontSize = 18, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            selectedFrame = MakeTexture(new Color(1f, 0.78f, 0.18f));
            normalFrame = MakeTexture(new Color(0.28f, 0.32f, 0.42f));
            portugalFlag = MakePortugalFlag();
            unitedKingdomFlag = MakeUnitedKingdomFlag();
        }

        private static Texture2D MakeTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private static Texture2D MakePortugalFlag()
        {
            const int width = 90, height = 54;
            var texture = new Texture2D(width, height);
            for (var y = 0; y < height; y++) for (var x = 0; x < width; x++)
            {
                var color = x < 36 ? new Color(0.02f, 0.40f, 0.20f) : new Color(0.84f, 0.08f, 0.12f);
                var dx = x - 36; var dy = y - height / 2;
                if (dx * dx + dy * dy < 121) color = new Color(0.98f, 0.78f, 0.08f);
                texture.SetPixel(x, y, color);
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D MakeUnitedKingdomFlag()
        {
            const int width = 90, height = 54;
            var texture = new Texture2D(width, height);
            for (var y = 0; y < height; y++) for (var x = 0; x < width; x++)
            {
                var diagonalA = Mathf.Abs(y - x * (height - 1f) / (width - 1f));
                var diagonalB = Mathf.Abs(y - (height - 1f - x * (height - 1f) / (width - 1f)));
                var color = new Color(0.05f, 0.16f, 0.42f);
                if (diagonalA < 5f || diagonalB < 5f || Mathf.Abs(x - width / 2f) < 9f || Mathf.Abs(y - height / 2f) < 9f) color = Color.white;
                if (diagonalA < 2f || diagonalB < 2f || Mathf.Abs(x - width / 2f) < 4f || Mathf.Abs(y - height / 2f) < 4f) color = new Color(0.78f, 0.04f, 0.13f);
                texture.SetPixel(x, y, color);
            }
            texture.Apply();
            return texture;
        }
    }
}
