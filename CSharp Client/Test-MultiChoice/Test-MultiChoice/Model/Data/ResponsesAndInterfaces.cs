using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_MultiChoice.Model;

namespace Test_MultiChoice.Data
{
    public class BaseResponse
    {
        public bool Ok;
        public string Message;

        public override string ToString()
        {
            return Message;
        }
    }

    public delegate void ResponseHandler<T>(T response);

    public class GetAnimalsCountResponse : BaseResponse
    {
        public List<AnimalCount> Data;
    }

    public class GetAnimalsCountPerGroupResponse : BaseResponse
    {
        public List<AnimalCountPerGroup> Data;
    }

    public class GetAnimalsResponse : BaseResponse
    {
        public List<Animal> Data;
    }

    public interface GetAnimalsInterface
    {
        void GetAnimalsCount(ResponseHandler<GetAnimalsCountResponse> handler);
        void GetAnimalsCountPerGroup(ResponseHandler<GetAnimalsCountPerGroupResponse> handler);
        void GetAnimals(ResponseHandler<GetAnimalsResponse> handler);
    }

    public interface SetAnimalsInfo
    {
        void CreateAnimals(ResponseHandler<BaseResponse> handler);
        void UpdateLocations(List<Animal> animalsList, ResponseHandler<BaseResponse> handler);
    }

}
