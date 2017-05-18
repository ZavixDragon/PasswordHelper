using System;
using LiteIoCContainer.Tests.TestObjects;
using Xunit;

namespace LiteIoCContainer.Tests
{
    public class ContainerTests
    {
        private readonly Container _container;

        public ContainerTests()
        {
            _container = new Container();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveNonRegisteredType_InvalidOperationExceptionThrow()
        {
            Assert.Throws<InvalidOperationException>(() => _container.Resolve<SimpleObject>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveRegisteredConcreteType_RegisteredTypeReturned()
        {
            _container.Register<SimpleObject>(typeof(SimpleObject));

            var obj = _container.Resolve<SimpleObject>();

            Assert.Equal(typeof(SimpleObject), obj.GetType());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveRegisteredAbstractType_RegisteredTypeReturned()
        {
            _container.Register<ISimple>(typeof(SimpleObject));

            var obj = _container.Resolve<ISimple>();

            Assert.Equal(typeof(SimpleObject), obj.GetType());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterInterfaceAsTheResolution_ArgumentExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => _container.Register<ISimple>(typeof(ISimple)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterAbstractClassAsTheResolution_ArgumentExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => _container.Register<ISimple>(typeof(SimpleAbstract)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveTypeTwice_DifferentInstancesOfType()
        {
            _container.Register<ISimple>(typeof(SimpleObject));

            var simple1 = _container.Resolve<ISimple>();
            var simple2 = _container.Resolve<ISimple>();

            Assert.NotEqual(simple1, simple2);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterNonMatchingTypes_ArgumentExceptionThrow()
        {
            Assert.Throws<ArgumentException>(() => _container.Register<CompositeObject>(typeof(SimpleObject)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterTypeAsItsBaseClassesInterface_ResolveSuccessfully()
        {
            _container.Register<IBase>(typeof(SimpleObject));

            var simple = _container.Resolve<IBase>();

            Assert.Equal(typeof(SimpleObject), simple.GetType());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterSameTypeWithAbstractAndConreteType_ResolveBothAsSameType()
        {
            _container.Register<SimpleObject>(typeof(SimpleObject));
            _container.Register<ISimple>((typeof(SimpleObject)));

            var simple1 = _container.Resolve<SimpleObject>();
            var simple2 = _container.Resolve<ISimple>();

            Assert.Equal(simple1.GetType(), simple2.GetType());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveObjectThatConstructorCannotBeResolved_InvalidOperationExceptionThrown()
        {
            _container.Register<CompositeObject>(typeof(CompositeObject));

            Assert.Throws<InvalidOperationException>(() => _container.Resolve<CompositeObject>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RegisterObjectWithConstructorArgs_ArgsNotNull()
        {
            _container.Register<ISimple>(typeof(SimpleObject));
            _container.Register<CompositeObject>(typeof(CompositeObject));

            var composite = _container.Resolve<CompositeObject>();

            Assert.NotNull(composite.Simple);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TypeWithMultipleConstructorsResolved_ResolvesOnlyViableOne()
        {
            _container.Register<MultiConstructorObject>(typeof(MultiConstructorObject));

            var multiConstructorObj = _container.Resolve<MultiConstructorObject>();

            Assert.True(multiConstructorObj.wasDefaultConstructorUsed);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TypeWithMultipleConstructorsResolved_ResolvesTheOneWithMoreArguments()
        {
            _container.Register<MultiConstructorObject>(typeof(MultiConstructorObject));
            _container.Register<ISimple>(typeof(SimpleObject));

            var multiConstructorObj = _container.Resolve<MultiConstructorObject>();

            Assert.False(multiConstructorObj.wasDefaultConstructorUsed);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ResolveRegisteredInstanceTwice_BothInstanceAreTheSameReference()
        {
            _container.RegisterInstance<ISimple>(new SimpleObject());

            var simple1 = _container.Resolve<ISimple>();
            var simple2 = _container.Resolve<ISimple>();

            Assert.True(ReferenceEquals(simple1, simple2));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AskIfNonRegisteredTypeIsRegistered_ReturnsFalse()
        {
            var isRegistered = _container.IsRegistered<ISimple>();

            Assert.False(isRegistered);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AskIfRegisteredTypeIsRegistered_ReturnsTrue()
        {
            _container.Register<ISimple>(typeof(SimpleObject));

            var isRegistered = _container.IsRegistered<ISimple>();

            Assert.True(isRegistered);
        }
    }
}
