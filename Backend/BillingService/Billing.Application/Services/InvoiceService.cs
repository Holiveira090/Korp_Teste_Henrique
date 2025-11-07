using AutoMapper;
using Billing.Application.Clients;
using Billing.Application.DTOs;
using Billing.Application.Services.Interfaces;
using Billing.Domain.Interfaces;
using Billing.Domain.Models;
using Billing.Domain.Models.Enums;
using System;
using System.Threading.Tasks;

namespace Billing.Application.Services
{
    public class InvoiceService : ServiceBase<InvoiceDTO, Invoice>, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IStockClient _stockClient;

        public InvoiceService(IInvoiceRepository invoiceRepository, IMapper mapper, IStockClient stockClient)
            : base(invoiceRepository, mapper)
        {
            _invoiceRepository = invoiceRepository;
            _stockClient = stockClient;
        }

        public override async Task<InvoiceDTO> CreateAsync(InvoiceDTO dto)
        {
            dto.SequentialNumber = await _invoiceRepository.GetNextSequentialNumberAsync();
            return await base.CreateAsync(dto);
        }

        public async Task<InvoiceDTO?> GetBySequentialNumberAsync(int sequentialNumber)
        {
            var invoice = await _invoiceRepository.GetBySequentialNumberAsync(sequentialNumber);
            return _mapper.Map<InvoiceDTO?>(invoice);
        }

        public async Task<int> GetNextSequentialNumberAsync()
        {
            return await _invoiceRepository.GetNextSequentialNumberAsync();
        }

        public async Task<InvoiceDTO?> GetInvoiceWithItemsAsync(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);
            return _mapper.Map<InvoiceDTO?>(invoice);
        }

        public async Task<InvoiceDTO> PrintInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(invoiceId);
            if (invoice == null)
                throw new Exception($"Nota fiscal {invoiceId} não encontrada.");

            if (invoice.Status != InvoiceStatus.Aberta)
                throw new Exception("Somente notas com status 'Aberta' podem ser impressas.");

            foreach (var item in invoice.Items)
            {
                var success = await _stockClient.UpdateStockAsync(item.ProductCode, -item.Quantity);
                if (!success)
                    throw new Exception($"Não foi possível atualizar o saldo do produto {item.ProductCode}");
            }

            invoice.Status = InvoiceStatus.Fechada;
            var updatedInvoice = await _invoiceRepository.UpdateAsync(invoice);

            return _mapper.Map<InvoiceDTO>(updatedInvoice);
        }
    }
}
