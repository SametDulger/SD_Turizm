using FluentValidation;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Validators
{
    public class HotelValidator : AbstractValidator<Hotel>
    {
        public HotelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Otel adı boş olamaz")
                .MaximumLength(200).WithMessage("Otel adı 200 karakterden uzun olamaz");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Otel kodu boş olamaz")
                .MaximumLength(20).WithMessage("Otel kodu 20 karakterden uzun olamaz")
                .Matches("^[A-Z0-9]+$").WithMessage("Otel kodu sadece büyük harf ve rakam içerebilir");

            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Konum 200 karakterden uzun olamaz");

            RuleFor(x => x.Stars)
                .InclusiveBetween(1, 5).WithMessage("Yıldız sayısı 1-5 arasında olmalıdır");

            RuleFor(x => x.RoomTypes)
                .MaximumLength(100).WithMessage("Oda tipleri 100 karakterden uzun olamaz");

            RuleFor(x => x.RoomLocations)
                .MaximumLength(100).WithMessage("Oda konumları 100 karakterden uzun olamaz");

            RuleFor(x => x.Amenities)
                .MaximumLength(500).WithMessage("Özellikler 500 karakterden uzun olamaz");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Telefon numarası 20 karakterden uzun olamaz");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("Web sitesi 200 karakterden uzun olamaz");
        }
    }
}
