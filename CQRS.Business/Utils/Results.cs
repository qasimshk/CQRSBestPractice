namespace CQRS.Business.Utils
{
    public enum State
    {
        ok,
        badRequest,
        notFound,
        unProcessableEntity,
        noContent
    }

    public class Results
    {
        public State Status { get; private set; }
        public bool IsSuccessful { get; private set; }
        public dynamic ResponseMessage { get; private set; }

        public Results SetResponse(State state, bool isSuccessful, dynamic response)
        {
            return new Results {
                 Status = state,
                 IsSuccessful = isSuccessful,
                 ResponseMessage = response
            };
        }
    }
}
