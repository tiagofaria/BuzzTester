# BuzzTester

Input tester for Buzz! Wireless V2 (PS3) buzzers. Plug in the dongle and see every button press in real time.

Projeto Unity independente para testar o recetor sem fios e os quatro comandos Buzz da PS3 no Windows.

## Utilização rápida

1. Liga o recetor USB Wbuzz ao computador.
2. Liga os comandos que têm pilhas; não é necessário ter os quatro ativos.
3. Executa `Builds/Windows/BuzzTester.exe`.
4. Confirma que o estado aparece a verde e carrega nos botões.
5. O botão **TESTAR LUZ** acende a luz vermelha desse jogador durante 1,5 segundos.

O projeto usa o recetor Sony com VID `054C` e PID `1000`. Se o recetor for removido, a aplicação tenta voltar a ligar automaticamente.

Para recompilar no Unity 6000.3.22f1, abre esta pasta e usa o menu **Buzz Tester > Build Windows**.
