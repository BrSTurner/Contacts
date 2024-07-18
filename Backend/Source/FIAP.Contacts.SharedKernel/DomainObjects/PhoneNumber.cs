using FIAP.Contacts.SharedKernel.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FIAP.Contacts.SharedKernel.DomainObjects
{
    public record PhoneNumber
    {
        [RegularExpression(@"^[9]\d{8}$", ErrorMessage = "Invalid cellphone number format. It must be a 9-digit number starting with 9.")]
        public string Number { get; init; }
        
        public int Code { get; init; }

        public PhoneNumber(int code, string number)
        {
            if(!IsValidPhoneCode(code))
                throw new ArgumentException("Invalid phone code.", nameof(code));

            if (!IsValidCellphone(number))
                throw new ArgumentException("Invalid cellphone number format. It must be a 9-digit number starting with 9.", nameof(number));
            
            Code = code;
            Number = number;
        }
        
        private static bool IsValidPhoneCode(int code) 
        {
            return PhoneCodes
                        .ValidCodes
                        .Values
                        .Any(codes => codes.Contains(code));
        } 

        private static bool IsValidCellphone(string number)
        {
            var regex = new Regex(@"^[9]\d{8}$");
            return regex.IsMatch(number);
        }

        public override string ToString() => Number;
    }
}
