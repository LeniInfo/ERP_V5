import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class WorkOrderService {

  private baseUrl = 'https://localhost:7231/api/v1/RepairOrder';

  constructor(private http: HttpClient) { }

  getByKey(fran: string, brch: string, workshop: string, repairType: string, repairNo: string) {
    return this.http.get(`${this.baseUrl}/${fran}/${brch}/${workshop}/${repairType}/${repairNo}`);
  }

  createWorkOrder(dto: any) {
    return this.http.post(this.baseUrl, dto);
  }

  updateWorkOrder(dto: any) {
    return this.http.put(this.baseUrl, dto);
  }

  deleteWorkOrder(fran: string, brch: string, workshop: string, repairType: string, repairNo: string) {
    return this.http.delete(`${this.baseUrl}/${fran}/${brch}/${workshop}/${repairType}/${repairNo}`);
  }

  // lookups
  getCustomers() { return this.http.get<any[]>('https://localhost:7231/api/v1/CustomerMaster'); }
  getWorks() { return this.http.get<any[]>('https://localhost:7231/api/v1/WorkMaster'); }
}
