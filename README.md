# APIsREST
Você deverá criar um programa de console em C# que faça requisições periódicas a um serviço REST e apresente, a cada leitura, a temperatura no horário exato, indicando se a temperatura subiu, desceu ou permaneceu igual em relação à leitura anterior. A saída deve usar cores no console: vermelho para “subiu”, azul para “desceu” e cor padrão para “sem alteração”.



Ao iniciar, o programa deve solicitar ao usuário: A unidade de temperatura desejada (Celsius, Kelvin ou Fahrenheit).
O intervalo em segundos para efetuar cada nova leitura.

Leitura Periódica

Após obter a unidade e o intervalo, o programa deve entrar em um laço que se repete indefinidamente (ou até o usuário interromper), fazendo requisições HTTP GET ao endpoint:http://localhost:5000/temperatura/{unidade} onde “{unidade}” é a escolha do usuário (celsius, kelvin ou fahrenheit).Cada resposta retornará um JSON contendo o valor da temperatura na unidade solicitada.

Registro de Horário
Para cada requisição bem-sucedida, capture o horário local exato (hora, minuto e segundo) em que a resposta foi obtida.

Comparação com Leitura Anterior

O programa deve manter em memória apenas o valor da última temperatura recebida.
Na primeira leitura, não há comparação; basta exibir o resultado normalmente.

Da segunda leitura em diante, compare o valor atual com o valor da leitura imediatamente anterior: 
Se o valor atual for maior que o anterior, considere “temperatura subiu”.
Se o valor atual for menor que o anterior, considere “temperatura desceu”.
Se for igual, considere “sem alteração”.

Saída no Console

A cada leitura, imprima uma linha contendo:
O horário da leitura no formato HH:mm:ss.
O valor da temperatura (com duas casas decimais) seguido da unidade escolhida.

Uma indicação de variação (“SUBIU”, “DESCEU” ou “SEM ALTERAÇÃO”) colorida:
Vermelho para “SUBIU”.
Azul para “DESCEU”.
Cor neutra (padrão) para “SEM ALTERAÇÃO”.

Exemplo ilustrativo (somente texto):

[14:07:32] Temperatura: 27,43 °C → SUBIU 
[14:07:37] Temperatura: 27,10 °C → DESCEU 
[14:07:42] Temperatura: 27,10 °C → SEM ALTERAÇÃO 
[14:07:47] Temperatura: 28,02 °C → SUBIU

Tratamento de Erros

Se a requisição HTTP falhar ou o serviço não estiver disponível, o programa deve exibir uma mensagem de erro (“Erro ao obter leitura”) em cor amarela (ou cor padrão, caso o console não suporte cores) e continuar aguardando o próximo ciclo.
Se a resposta JSON estiver em formato inesperado, exibir “Resposta inválida” e seguir adiante.

Interrupção do Programa

Permita que o usuário interrompa a execução a qualquer momento (por exemplo, pressionando Ctrl+C).
Ao receber o comando de encerramento, exiba uma mensagem simples (por exemplo: “Encerrando monitoramento de temperatura.”) e finalize a execução.
