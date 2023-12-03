using GameOfLife;

const int rows = 50;
const int columns = 80;
const int delay = 50;
var life = new Life(rows, columns);
var ui = new Ui(life, delay);
var configuration = ui.Greet();

BoardConfigurator.Configure(life, configuration);

do
{
	if (!ui.ShowLife())
		break;
}
while (life.Iterate());

ui.ShowLife(true);
ui.Bye();
