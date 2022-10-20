using Task1;

var field = new Field(9, 0, 3); //creating field 9x9 with numbers from 0 to 3

field.FillField();      //Filling field 


Console.WriteLine("Before");
field.ShowFieldConsole();

field.RemoveTriples();
