namespace FoundationBatch2
{
    public class Foundation
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Foundation Batch 2!");
            //Vatiables and Data Types
            int a=10;
            a="alads";
            string name="John";
            double pi=3.14;
            bool isTrue=true;
            const double gravity=9.8;
            Console.WriteLine(a);
            Console.WriteLine(name);
            Console.WriteLine(pi);
            Console.WriteLine(isTrue);
            Console.WriteLine(gravity);
            //Oparators
            //Operands Counts 
                //Unary Operators
                // increment and decrement operators
                 //post and pre increment and decrement
                //Binary Operators
                    //Arithmetic Operators
                int sum =10+20;
                int difference=20-10;
                int product=10*20;
                int quotient=20/10;
                int modulus=20%10;
                Console.WriteLine(sum);
                Console.WriteLine(difference);
                Console.WriteLine(product);
                Console.WriteLine(quotient);
                Console.WriteLine(modulus);
                    //Relational Operators
               Console.WriteLine(10>20);
               Console.WriteLine(10<20);
               Console.WriteLine(10==20);
               Console.WriteLine(10!=20);
               Console.WriteLine(10>=20);
               Console.WriteLine(10<=20);
                    //Logical Operators

                    /**
                    AND (&&) - Returns true if both operands are true.
                    TRUE && TRUE = TRUE
                    TRUE && FALSE = FALSE
                    FALSE && TRUE = FALSE
                    FALSE && FALSE = FALSE
                    OR (||) - Returns true if at least one operand is true.
                    TRUE || TRUE = TRUE
                    TRUE || FALSE = TRUE
                    FALSE || TRUE = TRUE
                    FALSE || FALSE = FALSE
                    NOT (!) - Returns the opposite of the operand's value.


                    **/

                    //Assignment Operators
                    // a= a+b;
                    //
                    //Bitwise Operators

                    
                //Ternary Operators
                int num1=10;
                int num2=20;
                int max=(num1>num2)?num1:num2;

            //Control Flow Statements
            if(num1>num2)
            {
                Console.WriteLine("num1 is greater than num2");
                if(num1>15)
                {
                    Console.WriteLine("num1 is greater than 15");
                }
                else
                {
                    Console.WriteLine("num1 is less than or equal to 15");
                }
            }
            else
            {
                if(num2>15)
                {
                    Console.WriteLine("num2 is greater than 15");
                }
                else
                {
                    Console.WriteLine("num2 is less than or equal to 15");
                }
                
            }
            //Else if Ladder
            int obtainedMarks=85;
            if(obtainedMarks>=90)
            {
                Console.WriteLine("Grade A");
            }
            else if(obtainedMarks>=80)
            {
                Console.WriteLine("Grade B");
            }
            else if(obtainedMarks>=70)
            {
                Console.WriteLine("Grade C");
            }
            else if(obtainedMarks>=60)
            {
                Console.WriteLine("Grade D");
            }
            else
            {
                Console.WriteLine("Grade F");
            }

//Switch Case
            int day=3;
            switch(day)
            {
                case 1:
                    Console.WriteLine("Monday");
                    break;
                case 2:
                    Console.WriteLine("Tuesday");
                    break;
                case 3:
                    Console.WriteLine("Wednesday");
                    break;
                case 4:
                    Console.WriteLine("Thursday");
                    break;
                case 5:
                    Console.WriteLine("Friday");
                    break;
                case 6:
                    Console.WriteLine("Saturday");
                    break;
                case 7:
                    Console.WriteLine("Sunday");
                    break;
                default:
                    Console.WriteLine("Invalid day");
                    break;
            }
            //Loops
            for(int i=0;i<5;i++)
            {
                Console.WriteLine("For Loop: "+i);
            }
            while(num1<15)
            {
                Console.WriteLine("While Loop: "+num1);
                num1++;
            }
            do
            {
                Console.WriteLine("Do-While Loop: "+num2);
                num2++;
            }while(num2<25);
           
            //Jumping Statements
            //Break, Continue, Return, Goto

            for(int i=0;i<10;i++)
            {
                if(i==5)
                {
                    break;
                }
                Console.WriteLine("Break Statement: "+i);
            }
            for(int i=0;i<10;i++)
            {
                if(i==5)
                {
                    continue;
                }
                Console.WriteLine("Continue Statement: "+i);
            }
            for(int i=0;i<10;i++)
            {
                if(i==5)
                {
                    return;
                }
                Console.WriteLine("Return Statement: "+i);
            }
            for(int i=0;i<10;i++)
            {
                if(i==5)
                {
                    goto Label;
                }
                Console.WriteLine("Goto Statement: "+i);
            }   
            //Type Conversion
        Console.WriteLine("Enter a number: ");
        string input = Console.ReadLine();  
        int number;
        if(int.TryParse(input, out number))
            {
                Console.WriteLine("You entered: "+number);
            }
            else
            {
                Console.WriteLine("Invalid input");
            }

            //
        }
    }
}