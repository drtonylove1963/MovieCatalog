using MediatR;
using MovieCatalog.Application.Common.Interfaces;
using MovieCatalog.Application.Common.Models;
using MovieCatalog.Application.Genres.Queries.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Application.Genres.Queries.GetGenresList;

public class GetGenresListQueryHandler : IRequestHandler<GetGenresListQuery, PaginatedList<GenreListItemDto>>
{
    private readonly IGenreReadRepository _genreReadRepository;

    public GetGenresListQueryHandler(IGenreReadRepository genreReadRepository)
    {
        _genreReadRepository = genreReadRepository;
    }

    public async Task<PaginatedList<GenreListItemDto>> Handle(GetGenresListQuery request, CancellationToken cancellationToken)
    {
        var genres = await _genreReadRepository.GetAllAsync(cancellationToken);
        
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            genres = genres.Where(g => 
                g.Name.Contains(request.SearchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                g.Description.Contains(request.SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        // Create paginated result
        var totalCount = genres.Length;
        var items = genres
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
            
        return new PaginatedList<GenreListItemDto>(
            items, 
            totalCount, 
            request.PageNumber, 
            request.PageSize);
    }
}
