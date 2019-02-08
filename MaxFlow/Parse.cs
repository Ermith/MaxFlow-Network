using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    class Parse
    {
        private void DisplayInvalidInput()
        {
            System.Windows.Forms.MessageBox.Show("Invalid input.");
        }
        
        public bool ValidateNonNegativeInteger(string s)
        {
            int num;
            bool validInput = int.TryParse(s, out num);
            if(validInput && num >= 0)
            {
                return true;
            }
            else
            {
                DisplayInvalidInput();
                return false;
            }
        }

        
    }
}
