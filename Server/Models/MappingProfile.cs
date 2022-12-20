using AutoMapper;
using CBPresents.Data.Models;
using CBPresents.Shared.Models;

namespace CBPresents.Server.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<LotteryEntry, LotteryEntryVM>();
        }
    }
}
