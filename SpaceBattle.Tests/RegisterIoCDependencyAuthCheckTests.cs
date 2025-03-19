using App;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyAuthCheckTests
    {
        public RegisterIoCDependencyAuthCheckTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            Ioc.Resolve<ICommand>("Scopes.Current.Set",
            Ioc.Resolve<object>("Scopes.New", Ioc.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void RegisterIoCDependencyAuthCheck_Successfully_Registers_Dependency()
        {
            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Action" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            Ioc.Resolve<ICommand>("IoC.Register",
                "Players.GetBelongings",
                new Func<object[], object>((object[] args) => (object)new List<string>())).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();

            Assert.NotNull(Ioc.Resolve<object>("Authorization.Check", "player1", "Action", "object1"));
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Action_Is_Own_And_Object_Belongs_To_Player()
        {
            var subjectId = "player1";
            var action = "Own";
            var objectId = "ship1";
            var playerBelongings = new List<string> { "ship1", "ship2" };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Players.GetBelongings",
                new Func<object[], object>((object[] args) => (object)playerBelongings)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Action_Is_Own_And_Object_Does_Not_Belong_To_Player()
        {
            var subjectId = "player1";
            var action = "Own";
            var objectId = "ship3";
            var playerBelongings = new List<string> { "ship1", "ship2" };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Players.GetBelongings",
                new Func<object[], object>((object[] args) => (object)playerBelongings)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.False(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Action_Is_Own_And_Belongings_Is_Null()
        {
            var subjectId = "player1";
            var action = "Own";
            var objectId = "ship1";

            Ioc.Resolve<ICommand>("IoC.Register",
                "Players.GetBelongings",
                new Func<object[], object?>((object[] args) => null)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.False(result);
        }
[Fact]
        public void AuthCheck_Returns_True_When_Player_Has_Global_Permissions()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "*", new List<string> { "Move", "Fire" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Object_Not_In_Permissions()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship2", new List<string> { "Move", "Fire" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.False(result);
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Object_Has_Wildcard_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "*" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_True_When_Object_Has_Specific_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Move", "Fire" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.True(result);
        }

        [Fact]
        public void AuthCheck_Returns_False_When_Object_Does_Not_Have_Specific_Permission()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            var permissions = new Dictionary<string, IEnumerable<string>>
            {
                { "ship1", new List<string> { "Fire", "Repair" } }
            };

            Ioc.Resolve<ICommand>("IoC.Register",
                "Authorization.GetPermissions",
                new Func<object[], object>((object[] args) => (object)permissions)).Execute();

            new RegisterIoCDependencyAuthCheck().Execute();
            var result = (bool)Ioc.Resolve<object>("Authorization.Check", subjectId, action, objectId);

            Assert.False(result);
        }
    }
}