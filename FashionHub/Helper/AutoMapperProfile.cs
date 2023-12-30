using AutoMapper;
using FashionHub.Data;
using FashionHub.ViewModels;


namespace FashionHub.Helper
{
    public class AutoMapperProfile: Profile
    {
		public AutoMapperProfile()
		{
			CreateMap<RegisterVM, User>();
			//.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
			//.ReverseMap();
		}
	}
}
	