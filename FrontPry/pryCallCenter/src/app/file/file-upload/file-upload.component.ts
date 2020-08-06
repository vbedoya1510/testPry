import { Component } from '@angular/core';
import { FileModel } from './models/file-model';
import { FileuploadService } from './services/file-upload-service';
import { FileUploadResponse } from './models/file-upload-response';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
})
export class FileUploadComponent {
  /**
   * uploaded file
   */
  public file: FileModel;

  /**
   * the api response
   */
  public fileResponse: FileUploadResponse;

  /**
   * message if api fails
   */
  public errorMessage: string;

  /**
   * final score for the service
   */
  public score: string;

  constructor(private uploadService: FileuploadService) {
    this.initComponents();
  }

  public initComponents(){
    this.file = {
      nombre: null,
      base64textString: null,
    };
    this.errorMessage = '';
  }

  /**
   * receive the file and read it
   */
  public selectFile(event) {
    this.initComponents();
    const files = event.target.files;
    const file = files[0];

    if (files && file) {

      const reader = new FileReader();
      if (this.isTxtFile(file.name)) {
        reader.onload = this._handleReaderLoaded.bind(this);
        reader.readAsBinaryString(file);
      } else {
        this.errorMessage = 'Solo se permite cargar archivos .txt';
      }
    }
  }

  /**
   * verify is the file have the correct extension
   */
  private isTxtFile(nameFile: string): boolean {
    return nameFile.endsWith(environment.fileExtension);
  }

  /**
   * convert content file to string
   */
  private _handleReaderLoaded(readerEvent) {
    const binaryString = readerEvent.target.result;
    this.file.base64textString = btoa(binaryString);
  }

  /**
   * check score and to assign the corresponding category
   */
  private finalScore(points: number) {
    if (points < 0) {
      this.score = '0 Estrellas';
    } else if (points < 25) {
      this.score = '1 Estrella';
    } else if (points >= 25 && points < 50) {
      this.score = '2 Estrellas';
    } else if (points >= 50 && points < 75) {
      this.score = '3 Estrellas';
    } else if (points >= 75 && points <= 90) {
      this.score = '4 Estrellas';
    } else if (points > 90) {
      this.score = '5 Estrellas';
    }
  }

  /**
   * call service and send the file
   */
  public upload() {
    this.uploadService.uploadFile(this.file).subscribe(
       datos => {
        if (datos.successful) {
          this.fileResponse = datos;
          this.finalScore(this.fileResponse.result);
        } else {
          this.errorMessage = 'Ocurrió un error al momento de calificar su servicio';
        }
      },
      (error) => {
        this.errorMessage = 'Ocurrió un error al momento de enviar el archivo';
      }
    );
  }

}
