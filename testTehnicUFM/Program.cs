using System;

namespace testTehnicUMT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            int changes = GetChangesNeeded(password);
            Console.WriteLine("Changes needed : {0}", changes);
        }

        /* 
         * This method take a string as input and returns the number
         * of changes needed to make it a strong password
         */
        static int GetChangesNeeded(string password)
        {
            int changes = 0;
            int changesDUL = 0; // Here we will note how many mandatory characters are not found in the password
            int changesRepeat = 0; // Here we will note how many replacements we have to do in the password
            bool hasLower = false;
            bool hasUpper = false;
            bool hasDigit = false;
            char prevChar = '\0';
            int repeat = 0;

            /*
             * This loops through each character in the password and checks
             * whether it is a lowercase letter, an uppercase letter, or a digit
             * If so, it sets the corresponding flag to true
             */
            foreach(char c in password) 
            {
                if(char.IsDigit(c)) 
                {  hasDigit = true;

                } else if(char.IsUpper(c)) 
                {  hasUpper = true;

                } else if(char.IsLower(c))
                {  hasLower = true;
                }


                // It keeps track of the number of repeated characters in a row.
                if(c == prevChar)
                {  repeat++;
                
                } else
                {  repeat = 1;
                }

                /*  
                 * If there are 3 or more repeted characters, it increments
                 * the changes counter
                 */
                if (repeat >= 3)
                {
                   changesRepeat++;
                   repeat = 0;
                }
                
                prevChar = c;
            }
            
            /*
             * Check the flags and if they are false, increments the changes
             * counter accordingly
             */
            if (!hasDigit)
                changesDUL++;
            if (!hasUpper)
                changesDUL++;
            if (!hasLower)
                changesDUL++;

            

            /*  
             * Dependind on the length of the entered password, we have 3 casese :
             * 1. length < 6;
             * 2. length >= 6 but length <= 20
             * 3. length > 20
             */
            if (password.Length < 6)
            {
                /*
                 * When length < 6, we have several distinct cases depending on
                 * the mandatory characters that must be found in the password 
                 * and groups of 3 characters
                 */

                /*
                 * In this case, if we have no repeating characters and we are missing
                 * a single mandatory character, it is enough to choose the highest number
                 * between changesDUL and the length that need to be addes to reach a
                 * minimum of 6
                 * An example : password : abcDe --> changesDUL = 1, 6-5 = 1 --> We have to
                 * make one change because if we put a digit, all the mandatory characters will
                 * be covered and the minimum of 6 characters will be reached
                 */
                if (changesDUL == 1 && changesRepeat == 0)
                    changes = Math.Max(changesDUL, 6 - password.Length);

                /*
                 * In this case, if we have repeating characters and we are missing
                 * a single mandatory character, we have to do the maximum between the 
                 * mandatory characters that still need to be added and the substitutions
                 * that we have to make, to which we add the number of characters that we still
                 * neetd to reach the minimum of 6;
                 * A replacement does not change the length of the password
                 * An example : password : aaa11 --> changesDUL = 1, changesRepeat = 1, 6-5 = 1 --> We have to
                 * make two change because if we put a digit, all the mandatory characters will
                 * be covered and we have to add another character to reach the minimum of 6 characters
                 */
                if (changesDUL == 1 && changesRepeat != 0)
                    changes = Math.Max(changesDUL, changesRepeat) + 6 - password.Length;
                
                /*
                 * When we have only to make replacements, the changes will be equal to how many replacements we
                 * need to make plus the characters necessary to reach a minimum of 6, because the 
                 * replacements do not change the length
                 * An example : aaa1B --> changesDUL = 0, changesRepeat = 1, 6-5 = 1 --> We have to make two changes
                 * because we have to replace a character and then add another one for a minimum of 6
                 */
                if (changesDUL == 0 && changesRepeat != 0)
                    changes = changesRepeat + 6 - password.Length;
                
                /*
                 * When we have to add more then one mandatory character but we also have to replace characters, we
                 * will use the maximim of those values
                 * An example : .///. --> changesDUL = 3, changesRepeat = 1 --> We have to make 3 changes because we
                 * have to replace a character and then add anothe two, to have all the mandatory characters
                 */
                if (changesDUL > 1 && changesRepeat != 0)
                    changes = Math.Max(changesDUL, changesRepeat);

                /*
                * We must add as many characters as necessary to reach a minimum of 6 characters
                * An example : // --> changesDUL = 3, changesRepeat = 0 --> We have to make 4 changes because we
                * have to add to add the three mandatory characters and one more to reach a minimum of 6 characters
                */
                if (changesDUL > 1 && changesRepeat == 0 || changesDUL == 0 && changesRepeat == 0)
                    changes = 6 - password.Length;
                

              /*
               * When we have length between six and twenty, we don't have to worry about the length, 
               * only how many mandatory characters we need and how many replacements
               * 
               * That's why the number of changes will be equal to the maximum of the number of mandatory
               * characters we need and the number of replacements
               * An example : aaaB1aaa --> changesDUL = 0, changesRepeat = 2 --> We have to make two changes because
               * we have to make two replacements
               */
            } else if (password.Length >= 6 && password.Length <= 20)
            {
                changes = Math.Max(changesDUL, changesRepeat);


            /*
             * When length > 20, we have several distinct cases depending on
             * the mandatory characters that must be found in the password 
             * and groups of 3 characters
             */
            }
            else if (password.Length > 20)
            {

                /*
                 * In a password of suitable length we will have a maximum number of replacements whitch is equal to 6,
                 * we can never have more. Therefore, if we have more than 6 replacements, we will not perform as many
                 * as there are because that part will always be cut and we will make additional changes that are not
                 * necessary
                 */
                if (changesRepeat >= 6)
                    changes = 6 + password.Length - 20;

               /*
                * If we do not have to add mandatory characters or replacements, the changes will be equal to the number
                * of characters that must be removed to have a length of 20
                */
                if (changesDUL == 0 && changesDUL == 0)
                    changes = password.Length - 20;


               /*
                * If we only have to add mandatory characters, their number will be added to the number of characters
                * to be removed
                */
                if (changesDUL != 0 && changesRepeat == 0)
                    changes = changesDUL + password.Length - 20;

                /*
                 * When we have to add mandatory characters but also to make replacements, no more the 6, we will choose
                 * the maximum of the variables above and at the end we will add the caracters that must be removed
                 */
                if (changesDUL != 0 && changesRepeat < 6)
                    changes = password.Length - 20 + Math.Max(changesDUL, changesRepeat);

            }

            
            return changes;
        }
    }
}
