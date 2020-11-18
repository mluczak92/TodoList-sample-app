using Autofac.Extras.Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;
using Xunit;

namespace TodoList_sample_app_tests.Models.Database {
    public class DatabaseMigratorTests {
        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        [InlineData(53)]
        public async Task ShouldThrowServerConnectionException(int sqlExceptionNumber) {
            using AutoMock mock = AutoMock.GetLoose();
            mock.Mock<IMigrateAsyncWrapper>()
                .Setup(x => x.MigrateAsync(It.IsAny<DatabaseFacade>()))
                .Throws(new SqlExceptionBuilder().WithErrorNumber(sqlExceptionNumber).Build());
            DatabaseMigrator migrator = mock.Create<DatabaseMigrator>();

            await Assert.ThrowsAsync<ServerConnectionException>(() => migrator.EnsureMigrated());
        }

        [Theory]
        [InlineData(-999)]
        [InlineData(999)]
        async public Task ShouldThrowMigrationException(int sqlExceptionNumber) {
            using AutoMock mock = AutoMock.GetLoose();
            mock.Mock<IMigrateAsyncWrapper>()
                .Setup(x => x.MigrateAsync(It.IsAny<DatabaseFacade>()))
                .Throws(new SqlExceptionBuilder().WithErrorNumber(sqlExceptionNumber).Build());
            DatabaseMigrator migrator = mock.Create<DatabaseMigrator>();

            await Assert.ThrowsAsync<MigrationException>(() => migrator.EnsureMigrated());
        }

        [Fact]
        public void ShouldNotFetch() {

        }

        [Fact]
        public void ShouldFetch() {

        }
    }
}
