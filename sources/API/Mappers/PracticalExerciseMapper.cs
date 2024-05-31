using Domain.Entities;
using Dommain.Entities;
using Model;

namespace API.Mappers
{
    public class PracticalExerciseMapper : AutoMapper.Profile
    {
        public PracticalExerciseMapper()
        {
            CreateMap<PracticalExercise, PracticalExerciseEntity>();
            CreateMap<PracticalExerciseEntity, PracticalExercise>();
        }
    }
}
