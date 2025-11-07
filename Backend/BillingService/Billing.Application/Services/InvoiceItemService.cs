using AutoMapper;
using Billing.Application.Clients;
using Billing.Application.DTOs;
using Billing.Application.Services;
using Billing.Application.Services.Interfaces;
using Billing.Domain.Interfaces;
using Billing.Domain.Models;

public class InvoiceItemService : ServiceBase<InvoiceItemDto, InvoiceItem>, IInvoiceItemService
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    private readonly IStockClient _stockClient;

    public InvoiceItemService(IInvoiceItemRepository invoiceItemRepository, IMapper mapper, IStockClient stockClient) : base(invoiceItemRepository, mapper)
    {
        _invoiceItemRepository = invoiceItemRepository;
        _stockClient = stockClient;
    }

    public override async Task<InvoiceItemDto> CreateAsync(InvoiceItemDto dto)
    {
        var productExists = await _stockClient.CheckProductExistsAsync(dto.ProductCode);
        if (!productExists)
            throw new Exception($"Produto com código {dto.ProductCode} não existe no estoque.");

        var currentStock = await _stockClient.GetStockQuantityAsync(dto.ProductCode);
        if (dto.Quantity > currentStock)
            throw new Exception($"Não é possível adicionar {dto.Quantity} unidades. Apenas {currentStock} disponíveis no estoque.");

        return await base.CreateAsync(dto);
    }

    public async Task<IEnumerable<InvoiceItemDto>> GetByInvoiceIdAsync(int invoiceId)
    {
        var items = await _invoiceItemRepository.GetByInvoiceIdAsync(invoiceId);
        return _mapper.Map<IEnumerable<InvoiceItemDto>>(items);
    }
}
