using AutoMapper;
using CoffeeAPI.Models;
using CoffeeAPI.DTO;

namespace CoffeeAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mapping (giữ nguyên)
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Users, UserRequestDto>().ReverseMap();
            CreateMap<Users, UserResponseDto>().ReverseMap();

            // Category Mapping (giữ nguyên)
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

            // Product Mapping
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));

            CreateMap<Product, ProductRequestDto>()
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));
            CreateMap<ProductRequestDto, Product>()
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));

            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes));

            // Ánh xạ cho ProductSize
            CreateMap<ProductSize, ProductSizeDto>().ReverseMap();

            // Cart Mapping
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));
            CreateMap<CartDto, Cart>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));

            CreateMap<Cart, CartRequestDto>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));
            CreateMap<CartRequestDto, Cart>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));

            CreateMap<Cart, CartResponseDto>()
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));

            // Order Mapping (giữ nguyên)
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));

            // OrderItem Mapping
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));
            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));

            CreateMap<OrderItem, OrderItemRequestDto>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));
            CreateMap<OrderItemRequestDto, OrderItem>()
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));

            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SelectedSize, opt => opt.MapFrom(src => src.SelectedSize))
                .ForMember(dest => dest.IceLevel, opt => opt.MapFrom(src => src.IceLevel))
                .ForMember(dest => dest.SugarLevel, opt => opt.MapFrom(src => src.SugarLevel));
        }
    }
}