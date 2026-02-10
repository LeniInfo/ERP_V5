export interface SupplierInvoice {
  header: HeaderDTO;
  details: DetailDTO[];
}

export interface HeaderDTO {
  vendor: string;
  doctype: string;
  docdt: string;
  docno: string;
}

export interface DetailDTO {
  part: number;
  make: string;
  qty: number;
  price: number;
  value: number;
  discount: number;
}
