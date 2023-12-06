﻿using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetAllManagePostQuery : IRequest<PaginatedList<GetAllPost>>
    {
        public string[] Categories { get; set; } = [];
        public string Search { get; set; } = "";
        public string Order { get; set; } = "date";
        public string Direction { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllManagePostHandler : IRequestHandler<GetAllManagePostQuery, PaginatedList<GetAllPost>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllManagePostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<GetAllPost>> Handle(GetAllManagePostQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.Posts.CountAsync();

            var pagingList = new PaginatedList<GetAllPost>(request.Categories, request.Search, request.Order, request.Direction, request.PageNumber, request.PageSize, count);

            if (count > 0)
            {
                var query = _context.Posts.AsQueryable();

                if (request.Order == "date")
                {
                    query = request.Direction == "desc" ? query.OrderByDescending(post => post.LastModifiedAt) : query.OrderBy(post => post.LastModifiedAt);
                }
                else if (request.Order == "view")
                {
                    query = request.Direction == "desc" ? query.OrderByDescending(post => post.View.Count) : query.OrderBy(post => post.View.Count);
                }
                else if (request.Order == "title")
                {
                    query = request.Direction == "desc" ? query.OrderByDescending(post => post.Title) : query.OrderBy(post => post.Title);
                }
                else if (request.Order == "published")
                {
                    query = request.Direction == "desc" ? query.OrderByDescending(post => post.IsPublished) : query.OrderBy(post => post.IsPublished);
                }

                if (request.Search != "")
                {
                    query = query.Where(post => post.Title.Contains(request.Search) || (post.Description != null && post.Description.Contains(request.Search)));
                }

                if (request.Categories.Length > 0)
                {
                    var categoryList = await _context.Categories
                    .ProjectTo<GetAllCategory>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);


                    var categoryIds = new HashSet<int>();

                    // get category id from slug and all category id of children
                    foreach (var slug in request.Categories)
                    {
                        var category = categoryList.FirstOrDefault(cate => cate.Slug == slug);
                        if (category != null)
                        {
                            categoryIds.Add(category.Id);

                            var children = categoryList.Where(cate => cate.ParentId == category.Id);
                            if (children != null)
                            {
                                AddChildrenIdToList(categoryIds, categoryList, children);
                            }
                        }
                    }

                    query = query.Where(post => post.PostCategories != null && post.PostCategories.Any(postCate => categoryIds.Contains(postCate.Category.Id)));
                }

                var pagingPosts = count > 0 ? await query
                    .Select(post => new GetAllPost
                    {
                        Id = post.Id,
                        Categories = post.PostCategories != null
                        ? post.PostCategories.Select(postCate => new GetAllCategory
                        {
                            Id = postCate.Category.Id,
                            Name = postCate.Category.Name,
                            Slug = postCate.Category.Slug
                        }) : null,
                        Title = post.Title,
                        Description = post.Description,
                        Thumbnail = post.Thumbnail != null ? post.Thumbnail : "/default.png",
                        IsPublished = post.IsPublished,
                        Slug = post.Slug,
                        View = post.View.Count,
                    })
                    .Skip((pagingList.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken) : null;

                pagingList.Items = pagingPosts;

            }

            return pagingList;
        }

        private void AddChildrenIdToList(HashSet<int> categoryIds, IList<GetAllCategory> categories, IEnumerable<GetAllCategory> childrenCategories)
        {
            foreach (var childCategory in childrenCategories)
            {
                categoryIds.Add(childCategory.Id);
                var children = categories.Where(cate => cate.ParentId == childCategory.Id);
                if (children != null)
                {
                    AddChildrenIdToList(categoryIds, categories, children);
                }
            }
        }
    }
}