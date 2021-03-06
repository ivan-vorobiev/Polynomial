﻿using System;
using System.Collections.Generic;

namespace PolynomialClassLibrary
{
    public class Polynomial
    {
        private List<ElementPolynomial> listElements;
        public Polynomial()
        {
            listElements = new List<ElementPolynomial>();
        }
        public Polynomial(int[] coefficients)
        {
            listElements = new List<ElementPolynomial>();
            int power = coefficients.Length - 1;

            for (int i = 0; i < coefficients.Length; i++)
                listElements.Add(new ElementPolynomial { Coefficient = coefficients[i], Power = power - i });
        }

        public Polynomial(List<int> coefficients)
        {
            listElements = new List<ElementPolynomial>();
            int power = coefficients.Count - 1;

            for (int i = 0; i < coefficients.Count; i++)
                listElements.Add(new ElementPolynomial { Coefficient = coefficients[i], Power = power - i });
        }

        private Polynomial(Polynomial a)
        {
            listElements = new List<ElementPolynomial>();

            foreach (var item in a.listElements)
                listElements.Add(new ElementPolynomial { Coefficient = item.Coefficient, Power = item.Power });
        }
        private Polynomial(int coefficient, int power)
        {
            listElements = new List<ElementPolynomial>();
            listElements.Add(new ElementPolynomial { Coefficient = coefficient, Power = power });
        }

        public override string ToString()
        {
            string result = "";

            foreach (var item in listElements)
            {
                if (item.Coefficient == 0)
                    result += "";

                else if (item.Coefficient == 1)
                {
                    if (item.Power == 1)
                        result += String.Format("X + ");
                    else if (item.Power == 0)
                        result += String.Format("{0}", item.Coefficient);
                    else
                        result += String.Format("X^{1} + ", item.Coefficient, item.Power);
                }
                else if (item.Coefficient == -1)
                {
                    if (item.Power == 1)
                        result += String.Format("-X + ");
                    else if (item.Power == 0)
                        result += String.Format("{0}", item.Coefficient);
                    else
                        result += String.Format("-X^{1} + ", item.Coefficient, item.Power);
                }
                else
                {
                    if (item.Power == 1)
                        result += String.Format("{0}X + ", item.Coefficient);
                    else if (item.Power == 0)
                        result += String.Format("{0}", item.Coefficient);
                    else
                        result += String.Format("{0}X^{1} + ", item.Coefficient, item.Power);
                }
            }

            if (result == "")
                return "0";
            else
                return result.Replace("+ -", "- ");
        }
        public static bool operator >(Polynomial a, Polynomial b)
        {
            if (a.listElements[0].Power > b.listElements[0].Power)
            {
                return true;
            }

            else if (a.listElements[0].Power == b.listElements[0].Power)
            {
                for(int i = 0; i < a.listElements.Count; i++)
                {
                    if (a.listElements[i].Coefficient > b.listElements[i].Coefficient)
                    {
                        return true;
                    }   
                }
            }
            return false;
        }

        public static bool operator <(Polynomial a, Polynomial b)
        {
            if (a.listElements[0].Power < b.listElements[0].Power)
            {
                return true;
            }

            else if (a.listElements[0].Power == b.listElements[0].Power)
            {
                for (int i = 0; i < a.listElements.Count; i++)
                {
                    if (a.listElements[i].Coefficient < b.listElements[i].Coefficient)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            Polynomial c;
            if (a > b)
            {
                c = new Polynomial(a);
                foreach (var i in c.listElements)
                {
                    foreach (var j in b.listElements)
                    {
                        if (i.Power == j.Power)
                            i.Coefficient += j.Coefficient;
                    }
                }
            }

            else
            {
                c = new Polynomial(b);
                foreach (var i in c.listElements)
                {
                    foreach (var j in a.listElements)
                    {
                        if (i.Power == j.Power)
                            i.Coefficient += j.Coefficient;
                    }
                }
            }

            return c;
        }
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            Polynomial c;
            if (a > b)
            {
                c = new Polynomial(a);
                foreach (var i in c.listElements)
                {
                    foreach (var j in b.listElements)
                    {
                        if (i.Power == j.Power)
                            i.Coefficient -= j.Coefficient;
                    }
                }
            }

            else
            {
                c = b * -1;
                foreach (var i in c.listElements)
                {
                    foreach (var j in a.listElements)
                    {
                        if (i.Power == j.Power)
                            i.Coefficient += j.Coefficient;
                    }     
                }
            }

            return c;
        }
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            Polynomial c = new Polynomial();

            foreach (var i in a.listElements)
            {
                foreach (var j in b.listElements)
                    c.listElements.Add(new ElementPolynomial { Coefficient = i.Coefficient * j.Coefficient, Power = i.Power + j.Power });
            }

            for (int i = 0; i < c.listElements.Count; i++)
            {
                for (int j = i + 1; j < c.listElements.Count; j++)
                    if (c.listElements[i].Power == c.listElements[j].Power)
                    {
                        c.listElements[i].Coefficient += c.listElements[j].Coefficient;
                        c.listElements.RemoveAt(j);
                    }
            }

            return c;
        }
        public static Polynomial operator *(Polynomial a, int b)
        {
            Polynomial c = new Polynomial();

            foreach (var i in a.listElements)
            {
                c.listElements.Add(new ElementPolynomial { Coefficient = i.Coefficient * b, Power = i.Power });
            }

            return c;
        }
        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            int tempCoeff, tempPower;
            Polynomial c = new Polynomial();

            int index = 0;
            while (a.listElements[index].Power >= b.listElements[0].Power)
            {
                tempPower = a.listElements[index].Power - b.listElements[0].Power;
                tempCoeff = a.listElements[index].Coefficient / b.listElements[0].Coefficient;

                a -= b * new Polynomial(tempCoeff, tempPower);

                c.listElements.Add(new ElementPolynomial { Coefficient = tempCoeff, Power = tempPower });
                index ++;
            }

            return c;
        }
        public double PointCalc(int point)
        {
            double result = 0;

            foreach (var item in listElements)
            {
                result += Math.Pow(point, item.Power) * item.Coefficient;
            }

            return result;
        }
    }
}