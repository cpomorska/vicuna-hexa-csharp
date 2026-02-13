using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace vicuna_infra_test.Arch
{
    public class BaseRules
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(Assembly.Load("vicuna-infra")).Build();

        private readonly IObjectProvider<Class> EntityClasses = Classes().That().AreAssignableTo(Types().That().HaveFullName("vicuna_ddd.Infrastructure.Shared.Entity.BaseEntity"));

        private readonly IObjectProvider<Interface> GenericInterfaces =
            Interfaces().That().HaveFullNameContaining("IGeneric").As("Generic Interfaces");


        [Fact]
        public void GenericClassesShouldHaveCorrectName()
        {
            Classes()
                .That().AreAssignableTo(GenericInterfaces)
                .Should().HaveNameContaining("Generic")
                .WithoutRequiringPositiveResults()
                .Check(Architecture);
        }


        [Fact]
        public void EntityClassesShouldResideInEntity()
        {
            Classes()
                .That().Are(EntityClasses)
                .Should().ResideInNamespace("..Domain..")
                .AndShould().ResideInNamespace("..Entity..")
                .WithoutRequiringPositiveResults()
                .Check(Architecture);
        }
    }
}