import { Injectable } from '@angular/core';

export interface ReportConfig {
  title: string;
  reportNumber: string;
  date: string;
  logo?: string;
  billedTo: {
    name: string;
    phone?: string;
    address?: string;
  };
  columns: ReportColumn[];
  data: any[];
  footer?: {
    companyName: string;
    phone?: string;
    email?: string;
    address?: string;
    signatureName?: string;
    signatureTitle?: string;
  };
  totalLabel?: string;
  status?: string; // 'Paid', 'Pending', 'Cancelled', etc.
  showGrandTotal?: boolean;
}

export interface ReportColumn {
  field: string;
  header: string;
  width?: string;
  align?: 'left' | 'center' | 'right';
  type?: 'text' | 'number' | 'currency';
  format?: (value: any) => string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  constructor() { }

  generateReport(config: ReportConfig): string {
    return this.buildReportHTML(config);
  }

  printReport(config: ReportConfig): void {
    const htmlContent = this.generateReport(config);
    const printWindow = window.open('', '_blank', 'width=800,height=600');

    if (printWindow) {
      printWindow.document.write(htmlContent);
      printWindow.document.close();

      // Wait for content to load before printing
      printWindow.onload = () => {
        printWindow.focus();
        printWindow.print();
      };
    }
  }

  downloadReport(config: ReportConfig, filename: string = 'report.html'): void {
    const htmlContent = this.generateReport(config);
    const blob = new Blob([htmlContent], { type: 'text/html' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  private buildReportHTML(config: ReportConfig): string {
    const styles = this.getStyles();
    const header = this.buildHeader(config);
    const table = this.buildTable(config);
    const footer = this.buildFooter(config);

    return `
      <!DOCTYPE html>
      <html lang="en">
      <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>${config.title}</title>
        <style>${styles}</style>
      </head>
      <body>
        <div class="report-container">
          <div class="diagonal-bg"></div>
          ${header}
          ${table}
          ${footer}
        </div>
      </body>
      </html>
    `;
  }

  private getStyles(): string {
    return `
      * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
      }

      body {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        background: #f5f5f5;
        padding: 20px;
      }

      .report-container {
        max-width: 900px;
        margin: 0 auto;
        background: white;
        box-shadow: 0 0 20px rgba(0,0,0,0.1);
        position: relative;
        overflow: hidden;
      }

      .diagonal-bg {
        position: absolute;
        top: 0;
        right: 0;
        width: 50%;
        height: 300px;
        background: linear-gradient(135deg, #6B8DD6 0%, #8E54E9 100%);
        clip-path: polygon(30% 0, 100% 0, 100% 100%, 0 100%);
        z-index: 1;
      }

      .diagonal-bg-bottom {
        position: absolute;
        bottom: 0;
        left: 0;
        width: 40%;
        height: 200px;
        background: linear-gradient(135deg, #6B8DD6 0%, #8E54E9 100%);
        clip-path: polygon(0 30%, 100% 100%, 0 100%);
        z-index: 1;
      }

      .report-header {
        padding: 40px 60px;
        position: relative;
        z-index: 2;
      }

      .header-top {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 40px;
      }

      .header-left {
        flex: 1;
      }

      .invoice-meta {
        color: #6B8DD6;
        font-size: 14px;
        margin-bottom: 5px;
      }

      .logo-container {
        width: 120px;
        height: 120px;
        background: white;
        border-radius: 15px;
        padding: 10px;
        box-shadow: 0 4px 10px rgba(0,0,0,0.1);
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .logo-container img {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain;
      }

      .report-title-section {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 30px;
      }

      .report-title {
        font-size: 72px;
        font-weight: 300;
        color: #6B8DD6;
        letter-spacing: 2px;
        text-transform: uppercase;
      }

      .status-badge {
        font-size: 48px;
        font-weight: bold;
        padding: 10px 30px;
        border-radius: 10px;
      }

      .status-paid {
        color: #4CAF50;
        background: rgba(76, 175, 80, 0.1);
      }

      .status-pending {
        color: #FF9800;
        background: rgba(255, 152, 0, 0.1);
      }

      .status-cancelled {
        color: #F44336;
        background: rgba(244, 67, 54, 0.1);
      }

      .billed-to {
        color: #666;
        line-height: 1.8;
      }

      .billed-to-label {
        color: #6B8DD6;
        font-weight: 600;
        margin-bottom: 5px;
      }

      .table-container {
        padding: 0 60px 40px;
        position: relative;
        z-index: 2;
      }

      .report-table {
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 0;
      }

      .report-table thead {
        background: linear-gradient(135deg, #6B8DD6 0%, #8E54E9 100%);
        color: white;
      }

      .report-table thead th {
        padding: 15px;
        text-align: left;
        font-weight: 600;
        font-size: 14px;
      }

      .report-table tbody tr {
        border-bottom: 1px solid #f0f0f0;
      }

      .report-table tbody tr:hover {
        background: #f9f9f9;
      }

      .report-table tbody td {
        padding: 15px;
        color: #6B8DD6;
        font-size: 14px;
      }

      .report-table tfoot {
        background: linear-gradient(135deg, #6B8DD6 0%, #8E54E9 100%);
        color: white;
      }

      .report-table tfoot td {
        padding: 20px 15px;
        font-size: 18px;
        font-weight: bold;
      }

      .text-left { text-align: left; }
      .text-center { text-align: center; }
      .text-right { text-align: right; }

      .report-footer {
        padding: 40px 60px 60px;
        display: flex;
        justify-content: space-between;
        align-items: flex-end;
        position: relative;
        z-index: 2;
      }

      .company-info {
        color: #6B8DD6;
        line-height: 1.8;
      }

      .company-name {
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 10px;
        font-style: italic;
      }

      .company-details {
        font-size: 14px;
        color: #666;
      }

      .signature-section {
        text-align: right;
      }

      .signature-image {
        width: 200px;
        height: 60px;
        margin-bottom: 10px;
      }

      .signature-line {
        width: 200px;
        border-top: 2px solid #6B8DD6;
        padding-top: 10px;
        color: #6B8DD6;
        font-size: 14px;
      }

      @media print {
        body {
          background: white;
          padding: 0;
        }

        .report-container {
          box-shadow: none;
          max-width: 100%;
        }
      }

      @media (max-width: 768px) {
        .report-header,
        .table-container,
        .report-footer {
          padding: 20px;
        }

        .report-title {
          font-size: 48px;
        }

        .status-badge {
          font-size: 32px;
        }

        .header-top {
          flex-direction: column;
        }

        .logo-container {
          margin-top: 20px;
        }

        .report-footer {
          flex-direction: column;
          align-items: flex-start;
          gap: 30px;
        }
      }
    `;
  }

  private buildHeader(config: ReportConfig): string {
    const logoHtml = config.logo
      ? `<div class="logo-container"><img src="${config.logo}" alt="Company Logo"></div>`
      : '';

    const statusHtml = config.status
      ? `<div class="status-badge status-${config.status.toLowerCase()}">${config.status}</div>`
      : '';

    return `
      <div class="report-header">
        <div class="header-top">
          <div class="header-left">
            <div class="invoice-meta">${config.title} #${config.reportNumber}</div>
            <div class="invoice-meta">Date : ${config.date}</div>
          </div>
          ${logoHtml}
        </div>
        
        <div class="report-title-section">
          <div class="report-title">${config.title.toUpperCase()}</div>
          ${statusHtml}
        </div>
        
        <div class="billed-to">
          <div class="billed-to-label">Billed To : ${config.billedTo.name}</div>
          ${config.billedTo.phone ? `<div>${config.billedTo.phone}</div>` : ''}
          ${config.billedTo.address ? `<div>${config.billedTo.address}</div>` : ''}
        </div>
      </div>
    `;
  }

  private buildTable(config: ReportConfig): string {
    const headers = config.columns.map(col =>
      `<th class="text-${col.align || 'left'}" style="${col.width ? `width: ${col.width}` : ''}">${col.header}</th>`
    ).join('');

    const rows = config.data.map(row => {
      const cells = config.columns.map(col => {
        let value = row[col.field];

        // Apply custom format if provided
        if (col.format) {
          value = col.format(value);
        } else if (col.type === 'currency') {
          value = `$${parseFloat(value || 0).toFixed(2)}`;
        } else if (col.type === 'number') {
          value = parseFloat(value || 0).toFixed(2);
        }

        return `<td class="text-${col.align || 'left'}">${value}</td>`;
      }).join('');

      return `<tr>${cells}</tr>`;
    }).join('');

    // Calculate total if needed
    let totalHtml = '';
    if (config.showGrandTotal !== false) {
      const totalColumn = config.columns.find(col => col.field.toLowerCase().includes('total'));
      if (totalColumn) {
        const total = config.data.reduce((sum, row) => sum + parseFloat(row[totalColumn.field] || 0), 0);
        const totalLabel = config.totalLabel || 'TOTAL';
        const colspan = config.columns.length - 1;
        totalHtml = `
          <tfoot>
            <tr>
              <td colspan="${colspan}" class="text-right">${totalLabel}</td>
              <td class="text-right">$${total.toFixed(2)}</td>
            </tr>
          </tfoot>
        `;
      }
    }

    return `
      <div class="table-container">
        <table class="report-table">
          <thead>
            <tr>${headers}</tr>
          </thead>
          <tbody>
            ${rows}
          </tbody>
          ${totalHtml}
        </table>
      </div>
    `;
  }

  private buildFooter(config: ReportConfig): string {
    if (!config.footer) return '<div class="diagonal-bg-bottom"></div>';

    const signatureHtml = config.footer.signatureName
      ? `
        <div class="signature-section">
          <div class="signature-image" style="font-family: 'Brush Script MT', cursive; font-size: 32px; color: #6B8DD6;">
            ${config.footer.signatureName}
          </div>
          <div class="signature-line">${config.footer.signatureTitle || ''}</div>
        </div>
      `
      : '';

    return `
      <div class="report-footer">
        <div class="company-info">
          <div class="company-name">${config.footer.companyName}</div>
          <div class="company-details">
            ${config.footer.phone ? `<div>${config.footer.phone}</div>` : ''}
            ${config.footer.email ? `<div>${config.footer.email}</div>` : ''}
            ${config.footer.address ? `<div>${config.footer.address}</div>` : ''}
          </div>
        </div>
        ${signatureHtml}
      </div>
      <div class="diagonal-bg-bottom"></div>
    `;
  }
}
