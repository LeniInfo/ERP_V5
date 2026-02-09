export interface PurchaseOrderDTO {
  header: HeaderDTO;
  details: DetailDTO[];
}

export interface HeaderDTO {
  franCode: string;
  branchCode: string;
  whCode: string;
  vendorCode: string;

  potype: string;
  pono: string;
  vendorrefno: string;

  currency: string;

  noofitems: number;
  discount: number;
  totalvalue: number;

  createdt: string;
  createby: string;
  createremarks: string;
}

export interface DetailDTO {
  franCode: string;
  branchCode: string;
  whCode: string;
  vendorCode: string;

  potype: string;
  pono: string;
  posrl: string;

  plantype: string;
  planno: number;
  plansrl: number;

  make: string;
  part: number;
  qty: number;
  unitprice: number;

  discount: number;
  vatpercentage: number;
  vatvalue: number;
  discountValue: number;
  totalValue: number;

  createdt: string;
  createby: string;
}

