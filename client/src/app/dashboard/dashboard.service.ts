import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseUrl=environment.apiUrl;

  constructor(private http: HttpClient) { }

 
}
