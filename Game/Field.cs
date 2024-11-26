using System.Windows.Media;

namespace Runer
{
    class Field
    {
        int field_x;
        int field_y;
        public Cell[,] field;

        public Field(int field_x, int field_y)
        {
            this.field_x = field_x;
            this.field_y = field_y;
            field = new Cell[field_y, field_x];
            for (int y = 0; y < field_y; y++)
            {
                for (int x = 0; x < field_x; x++)
                {
                    Color color;
                    if ((x + y) % 2 == 1)
                    {
                        color = Colors.LightGray;
                    }
                    else
                    {
                        //color = ConsoleColor.Black;
                        color = Colors.DarkGray;
                    }
                    field[y, x] = new Cell(color);
                }

            }

        }
        public Field(MapSize mapSize) : this(mapSize.Width, mapSize.Height) { }

        public Visible Look(int x, int y)
        {
            return field[y, x].Look();
        }

        public void AddAttack(int x, int y, Attack attack)
        {
            if (x >= 0 && y >= 0 && x < field_x && y < field_y)
            {
                field[y, x].AddAttack(attack);
            }
        }

        public void Tact()
        {
            for (int y = 0; y < field_y; y++)
            {
                for (int x = 0; x < field_x; x++)
                {
                    field[y, x].Tact();
                }
            }
        }
    }
}