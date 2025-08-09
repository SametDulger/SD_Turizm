using FluentValidation;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Validators
{
    public class VendorValidator : AbstractValidator<VendorDto>
    {
        public VendorValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Kod alanı boş olamaz")
                .Length(3, 20).WithMessage("Kod 3-20 karakter arasında olmalıdır")
                .Matches(@"^[A-Z0-9]+$").WithMessage("Kod sadece büyük harf ve rakam içerebilir");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim alanı boş olamaz")
                .Length(2, 100).WithMessage("İsim 2-100 karakter arasında olmalıdır")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("İsim sadece harf içerebilir");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon alanı boş olamaz")
                .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Geçerli bir telefon numarası giriniz")
                .Length(10, 20).WithMessage("Telefon numarası 10-20 karakter arasında olmalıdır");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .MaximumLength(100).WithMessage("E-posta 100 karakterden uzun olamaz");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres alanı boş olamaz")
                .Length(10, 200).WithMessage("Adres 10-200 karakter arasında olmalıdır");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Ülke alanı boş olamaz")
                .Length(2, 50).WithMessage("Ülke 2-50 karakter arasında olmalıdır");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz");

            RuleFor(x => x.VendorType)
                .NotEmpty().WithMessage("Tedarikçi türü boş olamaz")
                .Must(BeValidVendorType).WithMessage("Geçersiz tedarikçi türü");

            // Business logic validations
            RuleFor(x => x)
                .Must(HaveValidBusinessHours).WithMessage("İş saatleri geçerli olmalıdır")
                .Must(HaveValidContactInfo).WithMessage("En az bir iletişim bilgisi (telefon veya e-posta) gerekli");
        }

        private bool BeValidVendorType(string vendorType)
        {
            var validTypes = new[] { "Hotel", "TourOperator", "Airline", "Cruise", "TransferCompany", "RentACar", "Guide" };
            return validTypes.Contains(vendorType, StringComparer.OrdinalIgnoreCase);
        }

        private bool HaveValidBusinessHours(VendorDto vendor)
        {
            // Bu metod şimdilik true döndürüyor, gerçek implementasyonda business hours validation yapılabilir
            return true;
        }

        private bool HaveValidContactInfo(VendorDto vendor)
        {
            return !string.IsNullOrWhiteSpace(vendor.Phone) || !string.IsNullOrWhiteSpace(vendor.Email);
        }
    }
}
