using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZone.Core.Utils;

namespace GameZone.Core.DomainObjects
{
    public class ValidarErrosTestes
    {
        /// <summary>
        /// Validação de tamanho máximo de string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="maximum"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentLength(string stringValue, int maximum, string message)
        {
            int length = stringValue.Trim().Length;
            if (length > maximum)
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação de tamanho minimo e máximo (precisa estar entre os dois)
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentLength(string stringValue, int minimum, int maximum, string message)
        {
            int length = stringValue.Trim().Length;
            if (length < minimum || length > maximum)
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação de string se esta vazia
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentNotEmpty(string stringValue, string message)
        {
            if (stringValue == null || stringValue.Trim().Length == 0)
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se objeto é null
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se a data é a minima do .net
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentDateTimeMinValue(DateTime data, string message)
        {
            if (data.Equals(DateTime.MinValue))
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se a data é a máxima do .net
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentDateTimeMaxValue(DateTime data, string message)
        {
            if (data.Equals(DateTime.MaxValue))
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se é um email válido
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentValidEmail(string email, string message)
        {
            if (!StringUtils.IsValidEmail(email))
            {
                throw new DomainException(message);
            }
        }

        /// <summary>
        /// Validação se a Senha e o Confirmar Senham são iguais
        /// </summary>
        /// <param name="password"></param>
        /// <param name="rePassword"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentPasswordsMatch(string password, string rePassword, string message)
        {
            if (!password.Equals(rePassword))
            {
                throw new DomainException(message);
            }
        }
    }
}
