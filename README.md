# sgbd-merge-sort-join

No linux, rode com  
`mcs -out:bin/Join.exe $(find -name "*.cs")`  
`mono bin/Join.exe`

Importante: por conta de alguns imprevistos muitos chatos que ocorreram nessa semana, isso é, um dos participantes da equipe ter ficado bem doente e tendo que repousar, a aplicação foi levemente simplificada:

- Não usamos o conceito de página, trabalhando diretamente com as tuplas; Isso ainda respeita o limite máximo de tuplas na memória, já que:
- Utilizamos uma versão bem simplificada do external merge sort, em que trabalhamos apenas com duas tuplas na memória por vez. Basicamente pegamos um algoritmo de merge sort padrão, mas cada alteração que seria feito na estrutura de dados em memória (por ex: `A[i] = b`), fazemos em disco. Isso aumenta consideravelmente o número de operações de IO, já que não utilizamos blocos de memória, porém facilitou na hora de implementar o algoritmo.
- Além disso, a interface do Operador mudou, fazendo com que ao invés de passar como atributo um objeto da classe Tabela, passasse o caminho para onde o arquivo CSV referente à tabela está.
- Por conta do não uso de páginas, o número total de páginas geradas não é calculado.

Fora esses detalhes, o algoritmo funciona de forma ótima.