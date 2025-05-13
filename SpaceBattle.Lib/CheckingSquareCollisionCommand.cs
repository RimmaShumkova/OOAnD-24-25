using App;

namespace SpaceBattle.Lib
{
    public class CheckingSquareCollisionCommand : ICommand
    {
        private readonly IColliding collObj;

        public CheckingSquareCollisionCommand(IColliding collObj)
        {
            if (collObj is null)
            {
                throw new ArgumentNullException(nameof(collObj));
            }

            this.collObj = collObj;
        }

        public void Execute()
        {
            var neighborObj = Ioc.Resolve<IEnumerable<IColliding>>("Game.GetObjectsInSameSquare", collObj);
            neighborObj
                .Select(obj => Ioc.Resolve<ICommand>("Game.CollisionCommand", collObj, obj, Ioc.Resolve<ICommand>("Game.HandleCollision", collObj, obj)))
                .ToList()
                .ForEach(cmd => cmd.Execute());
        }
    }
}
