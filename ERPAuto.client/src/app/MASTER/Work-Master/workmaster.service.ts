import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WorkMaster } from './workmaster.model';

@Injectable({
  providedIn: 'root'
})
export class WorkMasterService {

  private baseUrl = 'https://localhost:7231/api/v1/WorkMaster';

  constructor(private http: HttpClient) {}

  getWorks(): Observable<WorkMaster[]> {
    return this.http.get<WorkMaster[]>(this.baseUrl);
  }

  createWork(work: WorkMaster): Observable<WorkMaster> {
    return this.http.post<WorkMaster>(this.baseUrl, work);
  }

  updateWork(work: WorkMaster): Observable<void> {
    return this.http.put<void>(this.baseUrl, work);
  }

  deleteWork(fran: string, workType: string, workId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${fran}/${workType}/${workId}`
    );
  }
}

