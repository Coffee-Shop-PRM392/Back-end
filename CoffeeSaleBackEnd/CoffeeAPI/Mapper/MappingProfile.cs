using AutoMapper;
using CoffeeAPI.Models;
using CoffeeAPI.DTO;

namespace CoffeeAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mapping
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Users, UserRequestDto>().ReverseMap();
            CreateMap<Users, UserResponseDto>().ReverseMap();

            // Category Mapping
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

            // Product Mapping
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductRequestDto>().ReverseMap();
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Cart Mapping
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<Cart, CartRequestDto>().ReverseMap();
            CreateMap<Cart, CartResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            // Order Mapping
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));

            // OrderItem Mapping
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}