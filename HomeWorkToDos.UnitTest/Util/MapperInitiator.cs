using AutoMapper;
using HomeWorkToDos.API.Mapper;

namespace HomeWorkToDos.UnitTest.Util
{
    /// <summary>
    /// Automapper initiator.
    /// </summary>
    public class MapperInitiator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperInitiator"/> class.
        /// </summary>
        protected MapperInitiator()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            Mapper = mappingConfig.CreateMapper();
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public IMapper Mapper { get; }
    }
}
