// Header info for supplier invoice
export interface SupplierInvoiceHeader {
  blDate: string; 
  blNumber: string; 
  branch: string; 
  buyerCode: string; 
  currency: string; 
  discountValue: number;
  eta: string; 
  fran: string; 
  noOfItems: number;
  shipCompanyCode: string; 
  shipmentDate: string; 
  shipmentNumber: string; 
  shipmentType: string; 
  shippingStatus: string; 
  supplierCode: string; 
  totalValue: number;
  vatValue: number;
  warehouseCode: string;

  details: SupplierInvoiceDetail[];
}

// Detail info for each item in a supplier invoice
export interface SupplierInvoiceDetail {
  branch: string;
  caseNo: string;
  containerNo: string;
  discount: number;
  discountValue: number;
  fran: string;
  make: string;
  ordPart: string;
  ordQty: number;
  part: string;
  poNo: string;
  poSrl: number;
  poType: string;
  qty: number;
  shipmentDate: string;
  shipmentNumber: string;
  shipmentSerial: number;
  shipmentType: string;
  supplier: string;
  totalValue: number;
  unitPrice: number;
  vatPercentage: number;
  vatValue: number;
  warehouseCode: string;
}


export interface PendingReceipt {

  sinvno: string;
  sinvdt: string;
  supplier: string;
  orderedQty: number;
  receivedQty: number;
  balanceQty: number;
}


// shipment-grn.model.ts

export interface ShipmentforGRN {
  fran: string;
  branch: string;
  warehouseCode: string;
  shipmentType: string;       // SINVTYPE
  shipmentNumber: string;     // SINVNO

  shipmentDate: string;       // use string for JSON DateOnly
  supplierCode: string;       // VENDOR
  currency: string;           // CURRENCY
  blNumber?: string | null;   // BLNO
  blDate?: string | null;     // BLDT
  buyerCode?: string | null;  // BUYERCODE
  shippingStatus?: string | null; // SHIPPINGSTATUS
  shipCompanyCode?: string | null; // SHIPCOMPANYCODE

  status?: string | null;
  noOfItems: number;

  details: ShipmentforGRNDetails[];
}

export interface ShipmentforGRNDetails {
  fran: string;
  branch: string;
  warehouseCode: string;
  shipmentType: string;
  shipmentNumber: string;
  shipmentSerial: number;
  shipmentDate: string;
  supplier: string;

  make: string;
  part: string;
  ordPart: string;

  qty: number;
  ordQty: number;
  unitPrice: number;
  totalValue: number;

  poType: string;
  poNo: string;
  poSrl: number;

  orderedQty: number;
  receivedQty: number;
  balanceQty: number;
}


// Payload structure to save GRN
export interface GrnSavePayload {
  sinvNo: string;       // Supplier Invoice Number
  receivedBy: string;   // User who received the goods
  receivedDate: string; // ISO date string of receipt
  details: {
    itemCode: string;   // Item Code
    receiveQty: number; // Quantity received
  }[];
}
