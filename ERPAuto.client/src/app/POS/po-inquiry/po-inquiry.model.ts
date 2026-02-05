export interface PoInquiry {
  id: number;
  franCode: string;
  branchCode: string;
  whCode: string;
  supplier: string;
  supplierName: string;
  pono: string;
  seqno: string;
  noOfItems: number;
  totalValue: number;
  discount: number;
  vat: number;
  createdt: string;
  phone?: string;
}
