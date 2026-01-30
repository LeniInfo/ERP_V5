import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

interface CustomerOrder {
  id?: number;
  orderNumber?: string;
  customerCode?: string;
  customerName?: string;
  customerPhone?: string;
  vehicleId?: string;
  vehiclePlate?: string;
  orderDate?: string;
  status?: string;
  totalAmount?: number;
  items?: any[];
  createdAt?: string;
  updatedAt?: string;
  [key: string]: any;
}

interface StatusParam {
  PARAMDESC?: string;
  paramDesc?: string;
  [key: string]: any;
}

@Component({
  selector: 'app-customer-order-list',
  standalone: true,
  imports: [CommonModule, DecimalPipe, DatePipe],
  templateUrl: './customer-order-list.component.html',
  styleUrls: ['./customer-order-list.component.css']
})
export class CustomerOrderListComponent implements OnInit {
  customerOrders: CustomerOrder[] = [];
  isLoadingOrders: boolean = false;
  statusOptions: StatusParam[] = [];
  isLoadingStatus: boolean = false;

  readonly customerOrderApiUrl = 'http://localhost:7231/api/v1/orders/CustomerOrders';
  readonly customerApiUrl = 'http://localhost:7231/api/v1/master/Customers';
  readonly statusApiUrl = 'https://localhost:7231/api/v1/finance/Params';
  readonly defaultFran = 'MAIN';
  readonly defaultBranch = 'MAIN';
  readonly defaultWarehouse = 'MAIN';
  readonly defaultCordType = 'CO';

  customers: any[] = [];
  showDetailModal: boolean = false;
  selectedOrderForDetail: CustomerOrder | null = null;

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadStatusOptions();
    this.loadCustomerOrders();
  }

  // Load status options from API
  loadStatusOptions(): void {
    this.isLoadingStatus = true;
    this.http.get<StatusParam[]>(this.statusApiUrl).subscribe({
      next: (data) => {
        this.statusOptions = data || [];
        this.isLoadingStatus = false;
        console.log('Status options loaded:', this.statusOptions.length);
      },
      error: (err) => {
        this.isLoadingStatus = false;
        console.error('Error loading status options:', err);
        // Fallback to default statuses if API fails
        this.statusOptions = [
          { PARAMDESC: 'OPEN' },
          { PARAMDESC: 'PENDING' },
          { PARAMDESC: 'COMPLETED' },
          { PARAMDESC: 'CANCELLED' }
        ];
      }
    });
  }

  // Get status display value
  getStatusValue(status: StatusParam): string {
    return status.PARAMDESC || status.paramDesc || '';
  }

  // Load all customer orders
  loadCustomerOrders(): void {
    this.isLoadingOrders = true;
    
    this.http.get<any[]>(`${this.customerOrderApiUrl}/headers`).subscribe({
      next: async (data) => {
        if (data && data.length > 0) {
          // First, load all customers to get names
          await this.loadCustomersForOrders();
          
          this.customerOrders = data.map((header: any) => {
            const customerCode = header.customer || header.Customer || '';
            // Find customer details from loaded customers list
            const customer = this.customers.find(c => c.customerCode === customerCode);
            
            return {
              id: undefined,
              orderNumber: header.cordNo || header.CordNo || '',
              customerCode: customerCode,
              customerName: customer?.name || customerCode,
              customerPhone: customer?.phone || 'N/A',
              vehicleId: '',
              vehiclePlate: '',
              orderDate: header.cordDate || header.CordDate || new Date().toISOString().split('T')[0],
              status: 'OPEN', // Default status
              totalAmount: header.totalValue || header.TotalValue || header.grossValue || header.GrossValue || 0,
              items: [],
              createdAt: header.createDt ? new Date(header.createDt).toISOString() : undefined,
              updatedAt: undefined,
              fran: header.fran || header.Fran || '',
              branch: header.branch || header.Branch || '',
              warehouse: header.warehouse || header.Warehouse || '',
              cordType: header.cordType || header.CordType || '',
              cordNo: header.cordNo || header.CordNo || '',
              grossValue: header.grossValue || header.GrossValue || 0,
              netValue: header.netValue || header.NetValue || 0,
              totalValue: header.totalValue || header.TotalValue || 0,
              discountValue: header.discountValue || header.DiscountValue || 0,
              vatValue: header.vatValue || header.VatValue || 0,
              noOfItems: header.noOfItems || header.NoOfItems || 0
            };
          });
        }
        this.isLoadingOrders = false;
        console.log('Customer orders loaded:', this.customerOrders.length);
      },
      error: (err) => {
        this.isLoadingOrders = false;
        if (err.status !== 0 && err.status !== 404) {
          console.error('Error loading customer orders:', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Failed to load customer orders from API',
            confirmButtonColor: '#d33'
          });
        } else {
          console.log('Customer Orders API not available');
        }
      }
    });
  }

  // Load customers for mapping to orders
  private loadCustomersForOrders(): Promise<void> {
    return new Promise((resolve) => {
      if (this.customers.length > 0) {
        resolve();
        return;
      }
      
      this.http.get<any[]>(this.customerApiUrl).subscribe({
        next: (data) => {
          this.customers = data || [];
          console.log('Customers loaded for order mapping:', this.customers.length);
          resolve();
        },
        error: (err) => {
          console.warn('Could not load customers for mapping:', err);
          resolve();
        }
      });
    });
  }

  // Navigate to add new order form
  goToAddOrder(): void {
    this.router.navigate(['./customer-order-form']);
  }

  // Navigate to edit order form
  goToEditOrder(order: CustomerOrder): void {
    this.router.navigate(['./customer-order-form'], {
      queryParams: {
        orderNo: order['cordNo'] || order.orderNumber,
        fran: order['fran'] || this.defaultFran,
        branch: order['branch'] || this.defaultBranch,
        warehouse: order['warehouse'] || this.defaultWarehouse,
        cordType: order['cordType'] || this.defaultCordType
      }
    });
  }

  // Delete a customer order
  deleteOrder(order: CustomerOrder): void {
    const fran = order['fran'] || this.defaultFran;
    const branch = order['branch'] || this.defaultBranch;
    const warehouse = order['warehouse'] || this.defaultWarehouse;
    const cordType = order['cordType'] || this.defaultCordType;
    const cordNo = order['cordNo'] || order.orderNumber || '';

    if (!cordNo) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Cannot delete order without order number',
        confirmButtonColor: '#d33'
      });
      return;
    }

    Swal.fire({
      title: 'Are you sure?',
      text: `Do you want to delete order ${cordNo}? This will also delete all line items.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        // First, delete all line items
        this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
          next: (lineItems) => {
            // Delete all line items first
            const deletePromises = (lineItems || []).map((item: any) => {
              const cordSrl = item.cordSrl || item.CordSrl || '';
              return this.http.delete(`${this.customerOrderApiUrl}/lines/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}/${cordSrl}`).toPromise();
            });

            Promise.all(deletePromises).then(() => {
              // Then delete the header
              this.http.delete(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
                next: () => {
                  Swal.fire({
                    icon: 'success',
                    title: 'Deleted!',
                    text: 'Order and all line items have been deleted',
                    confirmButtonColor: '#3085d6',
                    timer: 2000
                  });
                  this.loadCustomerOrders();
                },
                error: (err) => {
                  console.error('Error deleting order header:', err);
                  Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: err.error?.message || 'Failed to delete order header',
                    confirmButtonColor: '#d33'
                  });
                }
              });
            }).catch((err) => {
              console.error('Error deleting line items:', err);
              Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to delete some line items',
                confirmButtonColor: '#d33'
              });
            });
          },
          error: (err) => {
            // If we can't load line items, try to delete header anyway
            console.warn('Could not load line items, deleting header only:', err);
            this.http.delete(`${this.customerOrderApiUrl}/headers/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
              next: () => {
                Swal.fire({
                  icon: 'success',
                  title: 'Deleted!',
                  text: 'Order header has been deleted',
                  confirmButtonColor: '#3085d6',
                  timer: 2000
                });
                this.loadCustomerOrders();
              },
              error: (deleteErr) => {
                console.error('Error deleting order header:', deleteErr);
                Swal.fire({
                  icon: 'error',
                  title: 'Error',
                  text: deleteErr.error?.message || 'Failed to delete order',
                  confirmButtonColor: '#d33'
                });
              }
            });
          }
        });
      }
    });
  }

  // Show order details
  showDetail(order: CustomerOrder): void {
    this.selectedOrderForDetail = {
      ...order,
      items: order.items || []
    };
    this.showDetailModal = true;

    // Load line items from API
    const fran = order['fran'] || this.defaultFran;
    const branch = order['branch'] || this.defaultBranch;
    const warehouse = order['warehouse'] || this.defaultWarehouse;
    const cordType = order['cordType'] || this.defaultCordType;
    const cordNo = order['cordNo'] || order.orderNumber || '';

    if (cordNo && this.selectedOrderForDetail) {
      this.http.get<any[]>(`${this.customerOrderApiUrl}/lines/by-header/${fran}/${branch}/${warehouse}/${cordType}/${cordNo}`).subscribe({
        next: (lineItems) => {
          if (this.selectedOrderForDetail) {
            this.selectedOrderForDetail.items = (lineItems || []).map((item: any) => ({
              id: 0,
              partId: item.part?.toString() || item.Part?.toString() || '',
              partName: `Part ${item.part || item.Part || 'N/A'}`,
              make: item.make || item.Make || '',
              price: item.price || item.Price || 0,
              quantity: item.qty || item.Qty || 0,
              discount: item.discount || item.Discount || 0,
              total: item.totalValue || item.TotalValue || 0
            }));
          }
        },
        error: (err) => {
          console.error('Error loading line items for detail view:', err);
          if (this.selectedOrderForDetail) {
            this.selectedOrderForDetail.items = [];
          }
        }
      });
    }
  }

  closeDetailModal(): void {
    this.showDetailModal = false;
    this.selectedOrderForDetail = null;
  }

  // Get status badge class
  getStatusBadgeClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'COMPLETED':
        return 'bg-success';
      case 'PENDING':
      case 'OPEN':
        return 'bg-warning';
      case 'CANCELLED':
        return 'bg-danger';
      default:
        return 'bg-info';
    }
  }
}

