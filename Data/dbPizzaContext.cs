using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SperanzaPizzaApi.Models;

#nullable disable

namespace SperanzaPizzaApi.Data
{
    public partial class dbPizzaContext : DbContext
    {
        public dbPizzaContext()
        {
        }

        public dbPizzaContext(DbContextOptions<dbPizzaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DeliveryReport> DeliveryReports { get; set; }
        public virtual DbSet<DmAddress> DmAddresses { get; set; }
        public virtual DbSet<DmCity> DmCities { get; set; }
        public virtual DbSet<DmClientMessage> DmClientMessages { get; set; }
        public virtual DbSet<DmDeliveryRoute> DmDeliveryRoutes { get; set; }
        public virtual DbSet<DmLogEntry> DmLogEntries { get; set; }
        public virtual DbSet<DmOrder> DmOrders { get; set; }
        public virtual DbSet<DmOrderPayment> DmOrderPayments { get; set; }
        public virtual DbSet<DmOrderProduct> DmOrderProducts { get; set; }
        public virtual DbSet<DmOrderStatus> DmOrderStatuses { get; set; }
        public virtual DbSet<DmPaymentStatus> DmPaymentStatuses { get; set; }
        public virtual DbSet<DmProduct> DmProducts { get; set; }
        public virtual DbSet<DmProductCategory> DmProductCategories { get; set; }
        public virtual DbSet<DmProductPrice> DmProductPrices { get; set; }
        public virtual DbSet<DmProductSize> DmProductSizes { get; set; }
        public virtual DbSet<DmRole> DmRoles { get; set; }
        public virtual DbSet<DmStreet> DmStreets { get; set; }
        public virtual DbSet<DmToken> DmTokens { get; set; }
        public virtual DbSet<DmUser> DmUsers { get; set; }
        public virtual DbSet<DomainEntity> DomainEntities { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<GetNewId> GetNewIds { get; set; }
        public virtual DbSet<IncomingMessage> IncomingMessages { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<LinksExt> LinksExts { get; set; }
        public virtual DbSet<LinksIdent> LinksIdents { get; set; }
        public virtual DbSet<MessagesToSend> MessagesToSends { get; set; }
        public virtual DbSet<OutgoingMessage> OutgoingMessages { get; set; }
        public virtual DbSet<Structure> Structures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
               //optionsBuilder.UseSqlServer("Server=lab500.nc-one.com,1530;Database=dbPizza;User ID=sa;Password=KFaerZ2dZRiM;");
                optionsBuilder.UseSqlServer("Data source = SHAHZOD;initial catalog = dbPizza; integrated security = true;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DeliveryReport>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.Id, e.Result }, "ClusteredIndex-20190131@check93425")
                    .IsClustered();

                entity.HasIndex(e => e.MessageId, "NonClusteredIndex-20190131@check93501");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MessageId)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("messageId");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasColumnName("processDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Result).HasColumnName("result");
            });

            modelBuilder.Entity<DmAddress>(entity =>
            {
                entity.ToTable("dm_Addresses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("houseNumber");

                entity.Property(e => e.Lattitude)
                    .HasColumnType("decimal(18, 10)")
                    .HasColumnName("lattitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(18, 10)")
                    .HasColumnName("longitude");

                entity.Property(e => e.PostCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("postCode");

                entity.Property(e => e.StreetId).HasColumnName("streetId");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.DmAddresses)
                    .HasForeignKey(d => d.StreetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_Addresses_FK");
            });

            modelBuilder.Entity<DmCity>(entity =>
            {
                entity.ToTable("dm_Cities");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DmClientMessage>(entity =>
            {
                entity.ToTable("dm_ClientMessages");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("clientName");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("message");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .HasColumnName("phone");
            });

            modelBuilder.Entity<DmDeliveryRoute>(entity =>
            {
                entity.ToTable("dm_DeliveryRoutes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("addressId");

                entity.Property(e => e.Flat)
                    .HasMaxLength(10)
                    .HasColumnName("flat");

                entity.Property(e => e.GateCode)
                    .HasMaxLength(100)
                    .HasColumnName("gateCode");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.DmDeliveryRoutes)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_dm_Address");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DmDeliveryRoutes)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_dm_Order");
            });

            modelBuilder.Entity<DmLogEntry>(entity =>
            {
                entity.ToTable("dm_LogEntry");

                entity.HasIndex(e => e.AuthorizedUser, "idx_dm_LogEntry_authorizedUser");

                entity.HasIndex(e => e.EntityName, "idx_dm_LogEntry_entityName");

                entity.HasIndex(e => e.MessageId, "idx_dm_LogEntry_messageId");

                entity.HasIndex(e => e.Time, "idx_dm_LogEntry_time");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActionType).HasColumnName("actionType");

                entity.Property(e => e.AuthorizedUser).HasColumnName("authorizedUser");

                entity.Property(e => e.EntityId).HasColumnName("entityId");

                entity.Property(e => e.EntityName)
                    .HasMaxLength(256)
                    .HasColumnName("entityName");

                entity.Property(e => e.Info).HasColumnName("info");

                entity.Property(e => e.MessageId)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("messageId");

                entity.Property(e => e.ProxyId)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("proxyId");

                entity.Property(e => e.Success).HasColumnName("success");

                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TimeFinish)
                    .HasColumnType("datetime")
                    .HasColumnName("timeFinish")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<DmOrder>(entity =>
            {
                entity.ToTable("dm_Orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AsSoonAsPossible).HasColumnName("asSoonAsPossible");

                entity.Property(e => e.CashPayment).HasColumnName("cashPayment");

                entity.Property(e => e.ClientComment)
                    .HasMaxLength(256)
                    .HasColumnName("clientComment");

                entity.Property(e => e.ClientCommentIsRead).HasColumnName("clientCommentIsRead");

                entity.Property(e => e.ClientCompanyName)
                    .HasMaxLength(100)
                    .HasColumnName("clientCompanyName");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(200)
                    .HasColumnName("clientName");

                entity.Property(e => e.ClientPhone)
                    .HasMaxLength(45)
                    .HasColumnName("clientPhone");

                entity.Property(e => e.ClosedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("closedDate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.DeliveryCost)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("deliveryCost");

                entity.Property(e => e.EmployeeComment)
                    .HasMaxLength(256)
                    .HasColumnName("employeeComment");

                entity.Property(e => e.HasDelivery).HasColumnName("hasDelivery");

                entity.Property(e => e.HasInvoice).HasColumnName("hasInvoice");

                entity.Property(e => e.Nip)
                    .HasMaxLength(10)
                    .HasColumnName("nip");

                entity.Property(e => e.OrderIdentifier)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("orderIdentifier");

                entity.Property(e => e.OrderStatusId).HasColumnName("orderStatusId");

                entity.Property(e => e.ProductCost)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("productCost");

                entity.Property(e => e.СookingTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("сookingTime");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.DmOrders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_OrderStatus");
            });

            modelBuilder.Entity<DmOrderPayment>(entity =>
            {
                entity.ToTable("dm_OrderPayments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.ExtOrderId)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("extOrderId");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DmOrderPayments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_OrderPayments_FK");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.DmOrderPayments)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("dm_OrderPayments_FK2");
            });

            modelBuilder.Entity<DmOrderProduct>(entity =>
            {
                entity.ToTable("dm_OrderProducts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ProductCount).HasColumnName("productCount");

                entity.Property(e => e.ProductPriceId).HasColumnName("productPriceId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DmOrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_OrderProducts_FK");

                entity.HasOne(d => d.ProductPrice)
                    .WithMany(p => p.DmOrderProducts)
                    .HasForeignKey(d => d.ProductPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_OrderProducts_FK_1");
            });

            modelBuilder.Entity<DmOrderStatus>(entity =>
            {
                entity.ToTable("dm_OrderStatuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("statusName");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(100)
                    .HasColumnName("systemName");
            });

            modelBuilder.Entity<DmPaymentStatus>(entity =>
            {
                entity.ToTable("dm_PaymentStatuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PolandName)
                    .HasMaxLength(100)
                    .HasColumnName("polandName");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("statusName");
            });

            modelBuilder.Entity<DmProduct>(entity =>
            {
                entity.ToTable("dm_Products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Formulation)
                    .HasMaxLength(100)
                    .HasColumnName("formulation");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(255)
                    .HasColumnName("imagePath");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("productName");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DmProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_Products_categoryId");
            });

            modelBuilder.Entity<DmProductCategory>(entity =>
            {
                entity.ToTable("dm_ProductCategories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<DmProductPrice>(entity =>
            {
                entity.ToTable("dm_ProductPrices");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsPromo).HasColumnName("isPromo");

                entity.Property(e => e.PriceValue)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("priceValue");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.PromoEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("promoEndDate");

                entity.Property(e => e.PromoValue)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("promoValue");

                entity.Property(e => e.SizeId).HasColumnName("sizeId");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DmProductPrices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_ProductPrices_productId");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.DmProductPrices)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("dm_ProductPrices_sizeId");
            });

            modelBuilder.Entity<DmProductSize>(entity =>
            {
                entity.ToTable("dm_ProductSizes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");

                entity.Property(e => e.SizeName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("sizeName");

                entity.Property(e => e.SizeValue)
                    .HasMaxLength(45)
                    .HasColumnName("sizeValue");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.DmProductSizes)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_ProductSizes_FK");
            });

            modelBuilder.Entity<DmRole>(entity =>
            {
                entity.ToTable("dm_Roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<DmStreet>(entity =>
            {
                entity.ToTable("dm_Streets");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("streetName");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.DmStreets)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_Streets_FK");
            });

            modelBuilder.Entity<DmToken>(entity =>
            {
                entity.ToTable("dm_Tokens");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.ExpiredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expiredDate");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DmTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_Tokens_FK");
            });

            modelBuilder.Entity<DmUser>(entity =>
            {
                entity.ToTable("dm_Users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("firstName");

                entity.Property(e => e.IsVerified).HasColumnName("isVerified");

                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("lastName");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(45)
                    .HasColumnName("secondName");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.VerificationCode)
                    .HasMaxLength(100)
                    .HasColumnName("verificationCode");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.DmUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dm_Users_FK");
            });

            modelBuilder.Entity<DomainEntity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DomainEntities");

                entity.Property(e => e.EntityDescription).IsRequired();

                entity.Property(e => e.EntityType).IsRequired();
            });

            modelBuilder.Entity<Entity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Entities");

                entity.Property(e => e.Entity1).HasColumnName("Entity");
            });

            modelBuilder.Entity<GetNewId>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("getNewID");

                entity.Property(e => e.NewId).HasColumnName("new_id");
            });

            modelBuilder.Entity<IncomingMessage>(entity =>
            {
                entity.HasIndex(e => new { e.CreateDate, e.DeliveryState }, "NonClusteredIndex-20190201@check24918");

                entity.HasIndex(e => e.MessageId, "NonClusteredIndex-20190201@check24943");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasColumnName("author");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1024)
                    .HasColumnName("comment");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeliveryState).HasColumnName("deliveryState");

                entity.Property(e => e.MessageId)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("messageId");

                entity.Property(e => e.Offline).HasColumnName("offline");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("parentId");

                entity.Property(e => e.Payload).HasColumnName("payload");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Links");
            });

            modelBuilder.Entity<LinksExt>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("LinksExt");
            });

            modelBuilder.Entity<LinksIdent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("LinksIdents");
            });

            modelBuilder.Entity<MessagesToSend>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MessagesToSend");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("destination");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Payload).HasColumnName("payload");
            });

            modelBuilder.Entity<OutgoingMessage>(entity =>
            {
                entity.HasIndex(e => e.Noauto, "NonClusteredIndex-20190201@check25550");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1024)
                    .HasColumnName("comment");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("destination");

                entity.Property(e => e.Noauto).HasColumnName("noauto");

                entity.Property(e => e.Payload).HasColumnName("payload");
            });

            modelBuilder.Entity<Structure>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Structures");

                entity.Property(e => e.FieldName).HasMaxLength(128);

                entity.Property(e => e.FieldType).HasMaxLength(128);

                entity.Property(e => e.TableName).HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
