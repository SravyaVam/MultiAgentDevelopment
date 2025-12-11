# Cyber Underwriting Workbench Backend Requirements

## Business Overview
Build a cyber insurance underwriting platform that automates submission processing, quote generation, and risk assessment. The system processes insurance applications, applies business rules for referrals, and generates premium quotes based on company risk factors.

## Core Backend APIs

### Submission Intake API
- Process insurance submissions with company and coverage data
- Extract and validate submission fields (company info, revenue, NAICS, controls)
- Auto-create underwriting cases for each submission

### Case Management API
- Create and manage underwriting cases with unique Case IDs
- Track case status and decision history
- Link cases to audit logs and referral flags

### Quote Generation API
- Calculate premiums based on risk factors and rating tables
- Apply industry multipliers and control factors
- Return premium breakdown with referral flags and rule messages

### Referral Engine API
- Apply business rules for manual review triggers
- Validate security controls (MFA, EDR, backups)
- Generate referral explanations and rule messages

### Audit Trail API
- Track all decisions, changes, and UW overrides
- Maintain compliance logs for regulatory requirements
- Provide decision history for cases

## Data Models

### Submission Entity
- Company information (name, address, revenue, employee count)
- NAICS/SIC industry classification
- Requested coverages and limits
- Security controls and cyber posture data

### Case Entity
- Unique Case ID and creation timestamp
- Case status (New, In Review, Quoted, Referred)
- Linked submission and audit log references
- Referral flags and rule messages

### Coverage Models
- Standard coverages: Commercial Cyber Liability with policy aggregate limits
- Optional coverages: Business Income/Extra Expense with limits
- Coverage conditions and exclusions
- Waiting periods and deductibles

### Rating Tables
- Base rates by revenue bands (per $1,000 revenue)
- Industry risk multipliers by NAICS/SIC groups
- Controls factors for MFA, EDR, backups, patch management
- Data exposure factors based on PII/PHI/PCI record volumes

### Referral Rules
- Rule definitions with IDs (CYB-001 to CYB-010)
- Trigger conditions (MFA missing, exposure >1M, dark web detection)
- Rule categories (Identity & Access, Endpoint Security, Data Protection)

## Business Logic

### Premium Calculator
- Revenue-based base rating calculation
- Industry risk multiplier application
- Security controls factor adjustments
- Data exposure volume loading
- Minimum premium enforcement

### Referral Rules Engine
- MFA implementation validation
- Data exposure threshold checks (>1M records)
- Prior claims history analysis (within 3 years)
- Dark web credential detection flags
- Business Income/Extra Expense limit validation (>$2M)

### Risk Scoring
- Company risk profile assessment
- Security posture evaluation
- Industry-specific risk factors
- Controls maturity scoring

## API Endpoints

### Submission Management
- `POST /api/submissions` - Submit new insurance application
- `GET /api/submissions/{id}` - Get submission details
- `PUT /api/submissions/{id}` - Update submission data

### Case Management
- `GET /api/cases/{id}` - Get case details and status
- `GET /api/cases/submission/{submissionId}` - Get cases by submission
- `PUT /api/cases/{id}/status` - Update case status

### Quote Generation
- `POST /api/quotes/generate` - Generate premium quote
- `GET /api/quotes/{caseId}` - Get quote details
- `GET /api/quotes/{caseId}/breakdown` - Get premium factor breakdown

### Referral Management
- `GET /api/referrals/rules` - Get active referral rules
- `POST /api/referrals/evaluate` - Evaluate submission against rules
- `GET /api/referrals/cases` - Get referred cases

### Rating and Configuration
- `GET /api/rating/tables` - Get rating table data
- `GET /api/coverages/standard` - Get standard coverage options
- `GET /api/coverages/optional` - Get optional coverage options

## Technical Requirements
- .NET 8 Web API with Clean Architecture principles
- Entity Framework Core with InMemory database for MVP
- RESTful API design with proper HTTP status codes
- Swagger/OpenAPI documentation
- Structured logging and error handling
- Input validation and data sanitization
- Unit testing coverage (>80%)
- Security best practices implementation

## Sample Data Requirements
- NAICS industry codes with risk multipliers
- Revenue bands with base rates
- Standard coverage limits (50K, 100K, 250K, 500K)
- Optional coverage limits (500K, 1M, 2M, 3M)
- Referral rule definitions and trigger conditions
- Sample company submissions for testing

## Success Criteria
- All API endpoints are functional and properly documented
- Quote generation produces accurate premium calculations
- Referral rules engine correctly identifies risk factors
- Swagger UI provides interactive API documentation
- Unit tests achieve required coverage
- Code follows Clean Architecture patterns
- System handles error scenarios gracefully
- Audit trail captures all required compliance data