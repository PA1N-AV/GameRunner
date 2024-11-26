using System.Collections.Generic;
using System.Windows.Media;

namespace Runer
{
    class Cell
    {
        Color color;
        public List<Attack> attack = new List<Attack>();
        public Cell(Color color)
        {
            this.color = color;
        }

        public void Tact()
        {
            for (int i = 0; i < attack.Count; i++)
            {
                if (attack[i].Tact())
                {
                    attack.RemoveAt(i);
                    i--;
                }
            }
        }

        public int Status()
        {
            // на клітинці може не бути зон 
            // якщо небезпека є то вказати найбільшу з них
            // інакше вказати найменше попередження 
            //
            if (attack.Count == 0)
            {
                return 0;
            }
            {
                int timeattack = 0;
                for (int i = 0; i < attack.Count; i++)
                {
                    if (attack[i].Danger == 0)
                    {
                        if (timeattack < attack[i].TimeAttack)
                        {
                            timeattack = attack[i].TimeAttack;
                        }
                    }
                }
                if (timeattack > 0)
                {
                    return -timeattack;
                }
            }
            {
                int danger = attack[0].Danger;
                for (int i = 0; i < attack.Count; i++)
                {
                    if (danger > attack[i].Danger)
                    {
                        danger = attack[i].Danger;
                    }
                }
                return danger;
            }


            //int danger = 999;
            //int timeattack = 999;
            //for (int i = 0; i < attack.Count; i++)
            //{
            //
            //    if (danger > attack[i].Danger)
            //    {
            //        danger = attack[i].Danger;
            //    }
            //}
            //if (danger > 0 && danger != 999)
            //{
            //    return danger;
            //}
            //for (int i = 0; i < attack.Count; i++)
            //{
            //    if (timeattack > attack[i].TimeAttack)
            //    {
            //        timeattack = attack[i].TimeAttack;
            //    }
            //}
            //if (timeattack != 999)
            //{
            //    return -timeattack;
            //}
            //return 0;
        }

        public Visible Look()
        {
            Visible visible = new Visible();
            int status = Status();
            if (status == 0)
            {
                visible.Color = color;
                visible.Time = status;
            }
            if (status > 0)
            {
                visible.Color = Colors.DarkMagenta;
                visible.Time = status;
            }
            if (status < 0)
            {
                visible.Color = Colors.Red;
                visible.Time = -status;
            }
            return visible;
        }

        public void AddAttack(Attack attack)
        {
            this.attack.Add(attack);
        }

    }
}