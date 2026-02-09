
export interface ReceiptHeaderDto {
  fran: string;
  branch: string;
  warehouse: string;
  receiptType: string;
  receiptNo: string;
  receiptDate: string;   // yyyy-MM-dd
  noOfItems: number;
  netValue: number;
  currency: string;
  supplier: string;
  seqPrefix: string;
  seqNo: number;
  remarks?: string;
  status: string;
}

export interface CreateReceiptHeaderRequest {
  fran: string;
  branch: string;
  warehouse: string;
  receiptType: string;
  receiptNo: string;
  receiptDate: string;   // yyyy-MM-dd
  noOfItems: number;
  netValue: number;
  currency: string;
  supplier: string;
  seqPrefix: string;
  seqNo: number;
  remarks?: string;
  status: string;

  lines: CreateReceiptDetailRequest[];

}


export interface UpdateReceiptHeaderRequest {
  receiptDate?: string;
  noOfItems?: number;
  netValue?: number;
  currency?: string;
  supplier?: string;
  seqPrefix?: string;
  seqNo?: number;
  remarks?: string;
  status?: string;
}

export interface ReceiptDetailDto {
  fran: string;
  branch: string;
  warehouse: string;
  receiptType: string;
  receiptNo: string;
  receiptSerial: number;
  make: string;
  part: string;
  qty: number;
  unitPrice: number;
  netValue: number;
  currency: string;
  storeId: string;
  supplier: string;
  poType: string;
  poNo: string;
  poSrl: number;
  remarks?: string;
  status: string;
}

export interface CreateReceiptDetailRequest {
  fran: string;
  branch: string;
  warehouse: string;
  receiptType: string;
  receiptNo: string;
  receiptSerial: number;
  make: string;
  part: string;
  qty: number;
  unitPrice: number;
  netValue: number;
  currency: string;
  storeId: string;
  supplier: string;
  poType: string;
  poNo: string;
  poSrl: number;
  remarks?: string;
  status: string;
}

export interface UpdateReceiptDetailRequest {
  make?: string;
  part?: string;
  qty?: number;
  unitPrice?: number;
  netValue?: number;
  currency?: string;
  storeId?: string;
  supplier?: string;
  poType?: string;
  poNo?: string;
  poSrl?: number;
  remarks?: string;
  status?: string;
}
