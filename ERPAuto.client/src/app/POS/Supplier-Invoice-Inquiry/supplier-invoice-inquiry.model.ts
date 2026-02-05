export interface SupplierInvoiceInquiry {
  docno: string;
  docdt: string;
  vendorcode: string;
  vendorname: string;
  currency: string;
  shippingstatus: string;
  buyercode: string;
  blno: string;
  bldt: string;
  eta: string;
  noofitems: number;
  totalvalue: number;
  discountvalue: number;
  vatvalue: number;
  netvalue: number;
}
