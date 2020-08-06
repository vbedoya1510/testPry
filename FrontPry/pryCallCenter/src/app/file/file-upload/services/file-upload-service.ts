import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { FileModel } from '../models/file-model';
import { FileUploadResponse } from '../models/file-upload-response';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileuploadService {
  constructor(private http: HttpClient) {}

  uploadFile(file: FileModel): Observable<any> {
    return this.http.post(
      environment.apiURLUploadFile + 'file=' + file.base64textString,
      {headers: new HttpHeaders({ "Content-Type": "application/json"})}
    );
  }
}
