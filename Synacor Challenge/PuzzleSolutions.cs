using System.Diagnostics;

namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        public static int TeleporterSolver()
        {
            const int TARGET_VALUE = 6; //Mem_Read(5494);

            int regZero = 0;
            int regOne = 0;
            int regSeven = 0;

            Stack<int> stack = new(); 

            for (int testValue = 3; testValue <= MAIN_MEMORY_MAX; testValue++)
            {
                regSeven = testValue;
                Stopwatch sw = Stopwatch.StartNew();
                
                regZero = 4;
                regOne = 1;
                regSeven = testValue;
                //Merged(4, 1, testValue);   
                Func6027();

                sw.Stop();
                Console.WriteLine($"Reg[0] = {regZero}  Stopwatch {sw.ElapsedMilliseconds}");
                if (regZero == TARGET_VALUE) return testValue;
            }

            return -1;

            int Ackermann(int r0, int r1, int r7)   
            {
                // counting down to zero 
                if (r0 != 0)   //Func6035();               //    6027   jt reg[0]    6035              if reg[0] != 0 jump 6035
                {
                    //counting down to zero 
                    if (r1 != 0)                            //    6035   jt reg[1]    6048              if reg[1] != 0 jump 6048
                    {
                        //stack.Push(r0);                    //    6048 push reg[0]
                        
                        r1--; // (regOne + 32767) % MODULO   //    6050  add reg[1] reg[1]   32767       32767 as literal
                        r1 = Ackermann(r0, r1, r7);                               //    6054 call   6027                      
                        //r1 = r0;                       //    6056  set reg[1] reg[0]
                        //r0 = stack.Pop();                  //    6059  pop reg[0]
                        r0--; // = (regZero + 32767) % MODULO;   //    6061  add reg[0] reg[0]   32767       32767 as literal
                        r0 = Ackermann(r0, r1, r7);                               //    6065 call    6027                
                                                                //    6067  ret       
                    }
                    r0--; // = (regZero + 32767) % MODULO;       //    6038  add reg[0]  reg[0]   32767      reg[0]= reg[0] + literal value
                    r1 = r7;                          //    6042  set reg[1]  reg[7]              copy reg[7] to reg[1]
                    r0 = Ackermann(r0, r1, r7);                                   //    6045 call   6027                      recursive call.
                }
                return r1 + 1;                           //    6030  add reg[0]  reg[1]       1      add 1 + reg[1]
                                                                //    6034  ret          
            }


            void Func6027()
            {
                if (regZero != 0) Func6035();           //    6027   jt reg[0]    6035              if reg[0] != 0 jump 6035
                regZero = regOne + 1;                   //    6030  add reg[0]  reg[1]       1      add 1 + reg[1]
                                                        //    6034  ret          
            }

            void Func6035()
            {
                if (regOne != 0) Func6048();            //    6035   jt reg[1]    6048              if reg[1] != 0 jump 6048

                regZero = (regZero + 32767) % MODULO;   //    6038  add reg[0]  reg[0]   32767      reg[0]= reg[0] + literal value
                regOne = regSeven;                      //    6042  set reg[1]  reg[7]              copy reg[7] to reg[1]
                Func6027();                             //    6045 call   6027                      recursive call.
                                                        //    6047  ret
            }

            void Func6048()
            {
                stack.Push(regZero);                    //    6048 push reg[0]
                regOne = (regZero + 32767) % MODULO;    //    6050  add reg[1] reg[1]   32767       32767 as literal
                Func6027();                             //    6054 call   6027                      
                regOne = regZero;                       //    6056  set reg[1] reg[0]
                regZero = stack.Pop();                  //    6059  pop reg[0]
                regZero = (regZero + 32767) % MODULO;   //    6061  add reg[0] reg[0]   32767       32767 as literal
                Func6027();                             //    6065 call    6027                
                                                        //    6067  ret
            }
        }

        public static int OrbSolver()
        {
            return -1;
        }
    }
}

/*
Stack overflow.
Repeat 6020 times:
--------------------------------
   at Synacor_Challenge.Synacor9000.<TeleporterSolver>g__Func6027|28_0(<>c__DisplayClass28_0 ByRef)
   at Synacor_Challenge.Synacor9000.<TeleporterSolver>g__Func6048|28_2(<>c__DisplayClass28_0 ByRef)
   at Synacor_Challenge.Synacor9000.<TeleporterSolver>g__Func6035|28_1(<>c__DisplayClass28_0 ByRef)
--------------------------------
   at Synacor_Challenge.Synacor9000.<TeleporterSolver>g__Func6027|28_0(<>c__DisplayClass28_0 ByRef)
   at Synacor_Challenge.Synacor9000.TeleporterSolver()
   at Program.<<Main>$>g__DoConsoleCommand|0_5(System.String, <>c__DisplayClass0_0 ByRef)
   at Program.<Main>$(System.String[])
 */

/*
Stack overflow.
Repeat 16049 times:
--------------------------------
   at Synacor_Challenge.Synacor9000.<TeleporterSolver>g__Merged|28_0(<>c__DisplayClass28_0 ByRef)
--------------------------------
   at Synacor_Challenge.Synacor9000.TeleporterSolver()
   at Program.<<Main>$>g__DoConsoleCommand|0_5(System.String, <>c__DisplayClass0_0 ByRef)
   at Program.<Main>$(System.String[])
*/