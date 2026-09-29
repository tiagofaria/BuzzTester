# BuzzTester

Input tester for Buzz! Wireless V2 (PS3) buzzers. Plug in the dongle and see every button press in real time.

Projeto Unity independente para testar o recetor sem fios e os quatro comandos Buzz da PS3 no Windows.

## Utilização rápida

1. Descarrega o ficheiro ZIP da [release mais recente](https://github.com/tiagofaria/BuzzTester/releases/latest).
2. Extrai todo o conteúdo do ZIP para uma pasta.
3. Liga o recetor USB Wbuzz ao computador.
4. Liga os comandos que têm pilhas; não é necessário ter os quatro ativos.
5. Executa `BuzzTester.exe`.
6. Confirma que o estado aparece a verde e carrega nos botões.
7. O botão **TESTAR LUZ** acende a luz vermelha desse jogador durante 1,5 segundos.

O projeto usa o recetor Sony com VID `054C` e PID `1000`. Se o recetor for removido, a aplicação tenta voltar a ligar automaticamente.

## Compilar o projeto

Para recompilar a partir do código, abre o repositório no Unity 6000.3.22f1 e usa o menu **Buzz Tester > Build Windows**. A build será criada localmente em `Builds/Windows`.
