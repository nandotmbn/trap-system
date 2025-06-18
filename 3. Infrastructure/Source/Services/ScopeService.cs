using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
	public static class ScopeContainer
	{
		public static IServiceCollection ScopeService(this IServiceCollection services, IConfiguration configuration)
		{
			// var channel = RedisChannel.Literal("SignalR");

			// services.AddSignalR();
			// services.AddSignalR().AddStackExchangeRedis(configuration["Redis:ConnectionString"]!, options =>
			// {
			// 	options.Configuration.ChannelPrefix = channel;
			// 	options.Configuration.ConnectTimeout = 10000;
			// 	options.Configuration.SyncTimeout = 10000;
			// 	options.Configuration.KeepAlive = 180;
			// 	options.Configuration.DefaultDatabase = 0;
			// });

			// AppUser and Identity
			services.AddScoped<IAuth, AuthRepository>();
			services.AddScoped<IMine, MineRepository>();
			// services.AddScoped<IUser, UserRepository>();
			// services.AddScoped<ISecretKey, SecretKeyRepository>();
			// services.AddScoped<IRole, RoleRepository>();
			// services.AddScoped<IPermission, PermissionRepository>();
			// services.AddScoped<ICashier, CashierRepository>();
			// services.AddScoped<ICompanySetting, CompanySettingRepository>();
			// services.AddScoped<ICompanyPlanBuy, CompanyPlanBuyRepository>();
			// services.AddScoped<ICompanyXenPlatform, CompanyXenPlatformRepository>();
			// services.AddScoped<ICompanyBillSetting, CompanyBillSettingRepository>();

			// // Shift
			// services.AddScoped<IShift, ShiftRepository>();

			// // Content
			// services.AddScoped<IContentDelivery, ContentDeliveryRepository>();

			// // Item
			// services.AddScoped<IBrand, BrandRepository>();
			// services.AddScoped<ICategory, CategoryRepository>();
			// services.AddScoped<IColor, ColorRepository>();
			// services.AddScoped<IExtra, ExtraRepository>();
			// services.AddScoped<IExtraPrice, ExtraPriceRepository>();
			// services.AddScoped<IProduct, ProductRepository>();
			// services.AddScoped<IVariant, VariantRepository>();
			// services.AddScoped<IUnit, UnitRepository>();

			// // Inventory
			// services.AddScoped<IInventory, InventoryRepository>();
			// services.AddScoped<IOpnamedStock, OpnamedStockRepository>();
			// services.AddScoped<IWastedStock, WastedStockRepository>();
			// services.AddScoped<IMutationStock, MutationStockRepository>();
			
			// // Purchase Order
			// services.AddScoped<IPurchaseOrder, PurchaseOrderRepository>();
			// services.AddScoped<IPurchaseOrderItem, PurchaseOrderItemRepository>();
			// services.AddScoped<IGoodsReceipt, GoodsReceiptRepository>();
			// services.AddScoped<IGoodsReceiptItem, GoodsReceiptItemRepository>();
			// services.AddScoped<IInvoicePayment, InvoicePaymentRepository>();

			// // Sales Order
			// services.AddScoped<ISalesOrder, SalesOrderRepository>();
			// services.AddScoped<ISalesOrderItem, SalesOrderItemRepository>();
			// services.AddScoped<IDelivery, DeliveryRepository>();
			// services.AddScoped<IDeliveryItem, DeliveryItemRepository>();
			// services.AddScoped<IInvoice, InvoiceRepository>();

			// // Customer and Vendor
			// services.AddScoped<ICustomer, CustomerRepository>();
			// services.AddScoped<IVendor, VendorRepository>();

			// // Offers
			// services.AddScoped<IPackage, PackageRepository>();
			// services.AddScoped<IPromo, PromoRepository>();

			// // Programs
			// services.AddScoped<IGratuity, GratuityRepository>();
			// services.AddScoped<ITax, TaxRepository>();
			// services.AddScoped<ISalesMode, SalesModeRepository>();

			// // Point of Sales
			// services.AddScoped<ICashierCart, CashierCartRepository>();
			// services.AddScoped<ICashierOrder, CashierOrderRepository>();

			// // Finance and Accounting
			// services.AddScoped<IDepreciation, DepreciationRepository>();
			// services.AddScoped<IDepreciationMethod, DepreciationMethodRepository>();
			// services.AddScoped<IDetail, DetailRepository>();
			// services.AddScoped<IHeader, HeaderRepository>();
			// services.AddScoped<Interface.ITransaction, TransactionRepository>();
			// services.AddScoped<IJournal, JournalRepository>();
			// // services.AddScoped<IFinancialReport, FinancialReportRepository>();

			// // Xendit
			// services.AddScoped<IXenPlatform, XenPlatformRepository>();
			// services.AddScoped<IXenditInvoice, XenditInvoiceRepository>();

			// // Xendit Webhook
			// services.AddScoped<IXenditInvoiceWebhook, XenditInvoiceWebhookRepository>();

			// // Migrate
			// services.AddScoped<IMigrate, MigrateRepository>();

			return services;
		}
	}
}

