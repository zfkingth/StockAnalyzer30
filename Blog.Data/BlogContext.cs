using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> StockSet { get; set; }

        public DbSet<DayData> DayDataSet { get; set; }


        public DbSet<Sharing> SharingSet { get; set; }
        public DbSet<RealTimeData> RealTimeDataSet { get; set; }

        public DbSet<StockNum> StockNumSet { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<StockEvent> StockEvents { get; set; }
        public DbSet<SearchResult> SearchResultSet { get; set; }



        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //获取历史数据时，以单个股票为单位。
            modelBuilder.Entity<DayData>().HasKey(t => new
            {
                t.StockId,
                t.Date
            });

            //获取实时数据时，以最新地时间为重点
            modelBuilder.Entity<RealTimeData>().HasKey(t => new
            {

                t.StockId,
                t.Date,
            });
            modelBuilder.Entity<Sharing>().HasKey(t => new
            {
                t.StockId,
                t.DateGongGao
            });
            modelBuilder.Entity<StockNum>().HasKey(t => new
            {
                t.StockId,
                t.Date
            });

            modelBuilder.Entity<SearchResult>().HasKey(t => new
            {
                t.ActionName,
                t.ActionParams,
                t.ActionDate,
            });


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ConfigureModelBuilderForUser(modelBuilder);
            ConfigureModelBuilderForStory(modelBuilder);
            ConfigureModelBuilderForLike(modelBuilder);
            ConfigureModelBuilderForShare(modelBuilder);
        }

        void ConfigureModelBuilderForUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
        .Property(user => user.RoleName)
        .HasMaxLength(10)
        .IsRequired().HasDefaultValue("user");


            modelBuilder.Entity<User>()
            .Property(user => user.ExpiredDate)
            .IsRequired().HasDefaultValue(DateTime.MinValue);




            modelBuilder.Entity<User>()
               .Property(user => user.Id)
               .HasMaxLength(40)
               .IsRequired();


            modelBuilder.Entity<User>()
               .Property(user => user.Password)
               .HasMaxLength(90)
               .IsRequired();

            modelBuilder.Entity<StockEvent>()
                .Property(s => s.LastAriseEndDate)
                .IsRequired(false)
                .HasDefaultValue(null);
        }

        void ConfigureModelBuilderForStory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Story>().ToTable("Story");
            modelBuilder.Entity<Story>()
                .Property(s => s.Title)
                .HasMaxLength(100);

            modelBuilder.Entity<Story>()
                .Property(s => s.OwnerId)
                .IsRequired();

            modelBuilder.Entity<Story>()
                .HasOne(s => s.Owner)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.OwnerId);

            modelBuilder.Ignore<List<string>>();

        }

        void ConfigureModelBuilderForLike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().ToTable("Like");
            modelBuilder.Entity<Like>().HasKey(l => new { l.StoryId, l.UserId });
        }

        void ConfigureModelBuilderForShare(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Share>().ToTable("Share");
            modelBuilder.Entity<Share>().HasKey(l => new { l.StoryId, l.UserId });
        }

        public async Task TruncateRealTimeAndCacheTable()
        {
            await this.Database.ExecuteSqlRawAsync("truncate table RealTimeDataSet");
            await this.Database.ExecuteSqlRawAsync("truncate table SearchResultSet");

        }
    }
}