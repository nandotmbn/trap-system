using Infrastructure.Interfaces;
using Infrastructure.Database;
using Domain.Models;
using Domain.Types;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Domain.Errors;

namespace Infrastructure.Repositories;

public class ContentDeliveryRepository(AppDBContext appDBContext) : IContentDelivery
{
	async public Task<GetContentDeliveryByIdResponse> ArchiveContentDelivery(Guid? cdnId)
	{
		var isCDNExist = await appDBContext.ContentDeliveries.Where(x => x.Id == cdnId).FirstOrDefaultAsync();

		return isCDNExist == null
				? throw new NotFoundException("Content delivery is not found!")
				: new GetContentDeliveryByIdResponse(HttpStatusCode.OK, "Content delivery is successfully archived", isCDNExist);
	}

	async public Task<GetContentDeliveryByIdResponse> CreateContentDelivery(IFormFile file)
	{
		if (file == null || file.Length == 0)
			throw new BadRequestException("No file uploaded");

		var permalink = Path.Combine("CDN", Guid.NewGuid() + "_" + file.FileName);

		var filePath = Path.Combine(Directory.GetCurrentDirectory(), permalink);

		using var stream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(stream);

		var newCDN = new ContentDelivery
		{
			Title = file.FileName,
			Permalink = permalink,
		};

		appDBContext.ContentDeliveries.Add(newCDN);

		appDBContext.SaveChanges();
		return new GetContentDeliveryByIdResponse(HttpStatusCode.Created, "Content delivery is uploaded", newCDN);

	}

	async public Task<GetContentDeliveryByIdResponse> DeleteContentDelivery(Guid? cdnId)
	{
		var isCDNExist = await appDBContext.ContentDeliveries.Where(x => x.Id == cdnId).FirstOrDefaultAsync();

		if (isCDNExist == null) throw new NotFoundException("Content delivery is not found!");

		var filePath = Path.Combine(Directory.GetCurrentDirectory(), isCDNExist.Permalink);

		try
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				appDBContext.Remove(isCDNExist);
				appDBContext.SaveChanges();
				return new GetContentDeliveryByIdResponse(HttpStatusCode.Accepted, "Content delivery is successfully deleted", isCDNExist);
			}
			else
			{
				throw new NotFoundException("Filepath is not found!");
			}
		}
		catch (Exception ex)
		{
			throw new InternalServerErrorException("Internal server error: " + ex.Message);
		}
	}

	async public Task<GetContentDeliveriesResponse> GetContentDelivery(string? title, bool? isArchived, int? limit = 10, int? page = 1)
	{
		var query = appDBContext.ContentDeliveries.AsQueryable();
		int? itemsToSkip = (page - 1) * limit;
		if (title != null) query.Where(cdn => EF.Functions.Like(cdn.Title.ToLower(), $"%{title.ToLower().ToLower()}%"));

		if (isArchived != null)
		{
			query = query.Where(e => e.IsArchived == isArchived);
		}

		var cdns = await query.Skip((int)itemsToSkip!).Take((int)limit!).ToListAsync();
		return new GetContentDeliveriesResponse(HttpStatusCode.OK, "Content deliveries is successfully retrieved!", cdns);
	}

	async public Task<GetContentDeliveryByIdResponse> GetContentDeliveryById(Guid? cdnId)
	{
		var query = appDBContext.ContentDeliveries.Where(cdn => cdn.Id == cdnId && !cdn.IsArchived);

		var cdn = await query.FirstOrDefaultAsync() ?? throw new NotFoundException("Content delivery is not found!");
		return new GetContentDeliveryByIdResponse(HttpStatusCode.OK, "Content deliveries is successfully retrieved!", cdn);
	}

	async public Task<GetContentDeliveryByIdResponse> UpdateContentDelivery(Guid? cdnId, IFormFile file)
	{
		var isCDNExist = await appDBContext.ContentDeliveries.Where(x => x.Id == cdnId).FirstOrDefaultAsync();

		if (isCDNExist == null) throw new NotFoundException("Content delivery is not found!");

		var filePath = Path.Combine(Directory.GetCurrentDirectory(), isCDNExist.Permalink);

		try
		{
			if (File.Exists(filePath))
			{
				if (file == null || file.Length == 0)
					throw new BadRequestException("No file uploaded");

				var permalink = Path.Combine("CDN", Guid.NewGuid() + "_" + file.FileName);

				var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), permalink);

				using (var stream = new FileStream(newFilePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);

					isCDNExist.Title = file.FileName;
					isCDNExist.Permalink = permalink;

					await appDBContext.SaveChangesAsync();
					File.Delete(filePath);
					return new GetContentDeliveryByIdResponse(HttpStatusCode.Created, "Content delivery is updated!", isCDNExist);
				}

			}
			else
			{
				throw new NotFoundException("Filepath is not found!");
			}
		}
		catch (Exception ex)
		{
			throw new InternalServerErrorException("Internal server error: " + ex.Message);
		}
	}
}
