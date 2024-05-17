using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace vicuna_infra_test.Arch
{
    public class BaseRules
    {
        private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(System.Reflection.Assembly.Load("vicuna-infra")).Build();

        private readonly IObjectProvider<Interface> GenericInterfaces = Interfaces().That().HaveFullNameContaining("IGeneric").As("Generic Interfaces");

        private readonly IObjectProvider<Class> EntityClasses = Classes().That().AreAssignableTo("BaseEntity");


        [Fact]
        public void GenericClassesShouldHaveCorrectName() => Classes()
            .That().AreAssignableTo(GenericInterfaces)
            .Should().HaveNameContaining("Generic")
            .Check(Architecture);


        [Fact]
        public void EntityClassesShouldResideInEntity() => Classes()
            .That().Are(EntityClasses)
            .Should().ResideInNamespace("..Domain..")
            .AndShould().ResideInNamespace("..Entity..")
            .Check(Architecture);

    }
}
