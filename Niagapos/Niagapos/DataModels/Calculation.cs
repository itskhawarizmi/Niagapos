/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/

using System;

namespace Niagapos
{
    public class Calculation
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Calculation()
        {
            FirstOperand = string.Empty;
            SecondOperand = string.Empty;
            Operation = string.Empty;
            iResult = string.Empty;
        }

        public Calculation(string firstOperand, string secondOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperand(secondOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operation = operation;

            iResult = string.Empty;

            
        }

        public Calculation(string firstOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = String.Empty;

            Operation = operation;
            iResult = string.Empty;

        }

        #endregion

        #region Private Members

        private string iResult;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicating this's the first operand (value)
        /// </summary>
        public string FirstOperand { get; set; }

        /// <summary>
        /// Indicating this's the second operand (value)
        /// </summary>
        public string SecondOperand { get; set; }

        /// <summary>
        ///  The Operation for arithmetic or any calculations
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// The result to display 
        /// </summary>
        public string Result => iResult;

        #endregion

        #region Public Methods

        public void CalculateResult()
        {
            ValidateData();

            var result = string.Empty;

            try
            {
                switch (Operation)
                {
                    
                    case ("+"):
                        iResult = (Convert.ToDouble(FirstOperand) + Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("-"):
                        iResult = (Convert.ToDouble(FirstOperand) - Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("*"):
                        iResult = (Convert.ToDouble(FirstOperand) * Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("/"):
                        iResult = (Convert.ToDouble(FirstOperand) / Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("%"):
                        iResult = (Percentage(Convert.ToDouble(FirstOperand))).ToString();
                        break;

                    case ("pow"):
                        iResult = Math.Pow(Convert.ToDouble(FirstOperand), Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("!x"):
                        iResult = (Factorial(Convert.ToDouble(FirstOperand))).ToString();
                        break;

                    case ("log"):
                        iResult = Math.Log10(Convert.ToDouble(FirstOperand)).ToString();
                        break;

                    case ("sqrt"):
                        iResult = Math.Sqrt(Convert.ToDouble(FirstOperand)).ToString();
                        break;

                    case ("sin"):
                        iResult = Math.Sin(DegreeToRadian(Convert.ToDouble(FirstOperand))).ToString();
                        break;

                    case ("tan"):
                        iResult = Math.Tan(DegreeToRadian(Convert.ToDouble(FirstOperand))).ToString();
                        break;

                    case ("cos"):
                        iResult = Math.Cos(DegreeToRadian(Convert.ToDouble(FirstOperand))).ToString();
                        break;

                }
            }
            catch (Exception)
            {
                iResult = "Error while calculating";

                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "Error while calculating"
                });
            }
        }

        #endregion

        #region Private Methods

        private void ValidateData()
        {
            switch (Operation)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "pow":
                    ValidateOperand(FirstOperand);
                    ValidateOperand(SecondOperand);
                    break;
                case "%":
                case "log":
                case "sqrt":
                case "!x":
                case "tan":
                case "cos":
                case "sin":
                    ValidateOperand(FirstOperand);
                    break;

                default:
                    iResult = "Unknown operation: " + Operation;
                    DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Error",
                        Message = "Unknown operation"
                    });

                    return;
            }
        }

        /// <summary>
        /// Validation operand first whether the format as value is correct or not
        /// </summary>
        /// <param name="operand"></param>
        private void ValidateOperand(string operand)
        {
            try
            {
                Convert.ToDouble(operand);
            }

            catch (Exception)
            {
                iResult = "Invalid number: " + operand;
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "Invalid number"
                });
            }
        } 

        /// <summary>
        /// Validation operation can be allowed
        /// </summary>
        /// <param name="operation"></param>
        private void ValidateOperation(string operation)
        {
            switch (operation)
            {
                case ("+"):
                case ("-"):
                case ("*"):
                case ("/"):
                case ("%"):
                case ("tan"):
                case ("sin"):
                case ("cos"):
                case ("!x"):
                case ("sqrt"):
                case ("pow"):
                case ("log"):
                    break;

                default:
                    iResult = "Unknown operation: " + operation;
                    DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Error",
                        Message = "Unknown operation"
                    });

                    return;
            }
        }


        private double DegreeToRadian(double angle) => Math.PI * angle / 180.0;

        private double Percentage(double value) => value / 100;

        private double Factorial(double x)
        {
            if (x == 1)
                return 1;
            else
                return x * Factorial(x-1);

        }

        #endregion
    }
}

